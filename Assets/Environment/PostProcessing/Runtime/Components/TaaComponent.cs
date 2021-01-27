using System;

namespace UnityEngine.PostProcessing {
    public sealed class TaaComponent : PostProcessingComponentRenderTexture<AntialiasingModel> {
        private const string k_ShaderString = "Hidden/Post FX/Temporal Anti-aliasing";
        private const int k_SampleCount = 8;

        private readonly RenderBuffer[] m_MRT = new RenderBuffer[2];

        private RenderTexture m_HistoryTexture;
        private bool m_ResetHistory = true;

        private int m_SampleIndex;

        public override bool active =>
            model.enabled
            && model.settings.method == AntialiasingModel.Method.Taa
            && SystemInfo.supportsMotionVectors
            && SystemInfo.supportedRenderTargetCount >= 2
            && !context.interrupted;

        public Vector2 jitterVector { get; private set; }

        public override DepthTextureMode GetCameraFlags() {
            return DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
        }

        public void ResetHistory() {
            m_ResetHistory = true;
        }

        public void SetProjectionMatrix(Func<Vector2, Matrix4x4> jitteredFunc) {
            var settings = model.settings.taaSettings;

            var jitter = GenerateRandomOffset();
            jitter *= settings.jitterSpread;

            context.camera.nonJitteredProjectionMatrix = context.camera.projectionMatrix;

            if (jitteredFunc != null)
                context.camera.projectionMatrix = jitteredFunc(jitter);
            else
                context.camera.projectionMatrix = context.camera.orthographic
                    ? GetOrthographicProjectionMatrix(jitter)
                    : GetPerspectiveProjectionMatrix(jitter);

#if UNITY_5_5_OR_NEWER
            context.camera.useJitteredProjectionMatrixForTransparentRendering = false;
#endif

            jitter.x /= context.width;
            jitter.y /= context.height;

            var material = context.materialFactory.Get(k_ShaderString);
            material.SetVector(Uniforms._Jitter, jitter);

            jitterVector = jitter;
        }

        public void Render(RenderTexture source, RenderTexture destination) {
            var material = context.materialFactory.Get(k_ShaderString);
            material.shaderKeywords = null;

            var settings = model.settings.taaSettings;

            if (m_ResetHistory || m_HistoryTexture == null || m_HistoryTexture.width != source.width ||
                m_HistoryTexture.height != source.height) {
                if (m_HistoryTexture)
                    RenderTexture.ReleaseTemporary(m_HistoryTexture);

                m_HistoryTexture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
                m_HistoryTexture.name = "TAA History";

                Graphics.Blit(source, m_HistoryTexture, material, 2);
            }

            const float kMotionAmplification = 100f * 60f;
            material.SetVector(Uniforms._SharpenParameters, new Vector4(settings.sharpen, 0f, 0f, 0f));
            material.SetVector(Uniforms._FinalBlendParameters,
                new Vector4(settings.stationaryBlending, settings.motionBlending, kMotionAmplification, 0f));
            material.SetTexture(Uniforms._MainTex, source);
            material.SetTexture(Uniforms._HistoryTex, m_HistoryTexture);

            var tempHistory = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
            tempHistory.name = "TAA History";

            m_MRT[0] = destination.colorBuffer;
            m_MRT[1] = tempHistory.colorBuffer;

            Graphics.SetRenderTarget(m_MRT, source.depthBuffer);
            GraphicsUtils.Blit(material, context.camera.orthographic ? 1 : 0);

            RenderTexture.ReleaseTemporary(m_HistoryTexture);
            m_HistoryTexture = tempHistory;

            m_ResetHistory = false;
        }

        private float GetHaltonValue(int index, int radix) {
            var result = 0f;
            var fraction = 1f / radix;

            while (index > 0) {
                result += index % radix * fraction;

                index /= radix;
                fraction /= radix;
            }

            return result;
        }

        private Vector2 GenerateRandomOffset() {
            var offset = new Vector2(
                GetHaltonValue(m_SampleIndex & 1023, 2),
                GetHaltonValue(m_SampleIndex & 1023, 3));

            if (++m_SampleIndex >= k_SampleCount)
                m_SampleIndex = 0;

            return offset;
        }

        // Adapted heavily from PlayDead's TAA code
        // https://github.com/playdeadgames/temporal/blob/master/Assets/Scripts/Extensions.cs
        private Matrix4x4 GetPerspectiveProjectionMatrix(Vector2 offset) {
            var vertical = Mathf.Tan(0.5f * Mathf.Deg2Rad * context.camera.fieldOfView);
            var horizontal = vertical * context.camera.aspect;

            offset.x *= horizontal / (0.5f * context.width);
            offset.y *= vertical / (0.5f * context.height);

            var left = (offset.x - horizontal) * context.camera.nearClipPlane;
            var right = (offset.x + horizontal) * context.camera.nearClipPlane;
            var top = (offset.y + vertical) * context.camera.nearClipPlane;
            var bottom = (offset.y - vertical) * context.camera.nearClipPlane;

            var matrix = new Matrix4x4();

            matrix[0, 0] = 2f * context.camera.nearClipPlane / (right - left);
            matrix[0, 1] = 0f;
            matrix[0, 2] = (right + left) / (right - left);
            matrix[0, 3] = 0f;

            matrix[1, 0] = 0f;
            matrix[1, 1] = 2f * context.camera.nearClipPlane / (top - bottom);
            matrix[1, 2] = (top + bottom) / (top - bottom);
            matrix[1, 3] = 0f;

            matrix[2, 0] = 0f;
            matrix[2, 1] = 0f;
            matrix[2, 2] = -(context.camera.farClipPlane + context.camera.nearClipPlane) /
                           (context.camera.farClipPlane - context.camera.nearClipPlane);
            matrix[2, 3] = -(2f * context.camera.farClipPlane * context.camera.nearClipPlane) /
                           (context.camera.farClipPlane - context.camera.nearClipPlane);

            matrix[3, 0] = 0f;
            matrix[3, 1] = 0f;
            matrix[3, 2] = -1f;
            matrix[3, 3] = 0f;

            return matrix;
        }

        private Matrix4x4 GetOrthographicProjectionMatrix(Vector2 offset) {
            var vertical = context.camera.orthographicSize;
            var horizontal = vertical * context.camera.aspect;

            offset.x *= horizontal / (0.5f * context.width);
            offset.y *= vertical / (0.5f * context.height);

            var left = offset.x - horizontal;
            var right = offset.x + horizontal;
            var top = offset.y + vertical;
            var bottom = offset.y - vertical;

            return Matrix4x4.Ortho(left, right, bottom, top, context.camera.nearClipPlane, context.camera.farClipPlane);
        }

        public override void OnDisable() {
            if (m_HistoryTexture != null)
                RenderTexture.ReleaseTemporary(m_HistoryTexture);

            m_HistoryTexture = null;
            m_SampleIndex = 0;
            ResetHistory();
        }

        private static class Uniforms {
            internal static readonly int _Jitter = Shader.PropertyToID("_Jitter");
            internal static readonly int _SharpenParameters = Shader.PropertyToID("_SharpenParameters");
            internal static readonly int _FinalBlendParameters = Shader.PropertyToID("_FinalBlendParameters");
            internal static readonly int _HistoryTex = Shader.PropertyToID("_HistoryTex");
            internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");
        }
    }
}