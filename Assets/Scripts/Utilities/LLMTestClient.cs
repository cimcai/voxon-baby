using UnityEngine;
using Voxon.LLM;

namespace Voxon.Utilities
{
    /// <summary>
    /// Test client for LLM integration without requiring full system setup
    /// </summary>
    public class LLMTestClient : MonoBehaviour
    {
        [Header("Test Settings")]
        [SerializeField] private KeyCode testKey = KeyCode.T;
        [SerializeField] private string testPrompt = "What cat expression should I show if a human looks happy?";

        private LLMClient llmClient;
        private PromptBuilder promptBuilder;
        private LLMResponseParser responseParser;

        private void Start()
        {
            llmClient = FindObjectOfType<LLMClient>();
            promptBuilder = FindObjectOfType<PromptBuilder>();
            responseParser = FindObjectOfType<LLMResponseParser>();

            if (llmClient == null)
            {
                Debug.LogWarning("LLMClient not found. Create one to test LLM integration.");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(testKey))
            {
                TestLLMRequest();
            }
        }

        public void TestLLMRequest()
        {
            if (llmClient == null)
            {
                Debug.LogError("LLMClient not found. Cannot test LLM.");
                return;
            }

            string prompt = testPrompt;
            if (promptBuilder != null)
            {
                prompt = promptBuilder.BuildPrompt();
            }

            Debug.Log($"Sending LLM test request: {prompt}");

            llmClient.SendRequest(prompt, (response) =>
            {
                Debug.Log($"LLM Response: {response}");

                if (responseParser != null)
                {
                    var parsedResponse = responseParser.ParseResponse(response);
                    Debug.Log($"Parsed - Expression: {parsedResponse.expression}, " +
                             $"Intensity: {parsedResponse.intensity}, " +
                             $"Duration: {parsedResponse.duration}, " +
                             $"Reasoning: {parsedResponse.reasoning}");
                }
            });
        }

        private void OnGUI()
        {
            if (llmClient == null) return;

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 12;
            style.normal.textColor = Color.yellow;

            float yPos = Screen.height - 80;
            GUI.Label(new Rect(10, yPos, 400, 20), $"Press {testKey} to test LLM request", style);
            GUI.Label(new Rect(10, yPos + 20, 400, 20), $"Test Prompt: {testPrompt}", style);
        }
    }
}

