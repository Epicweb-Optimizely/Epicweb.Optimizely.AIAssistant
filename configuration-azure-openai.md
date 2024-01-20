 # Setup Azure OpenAI models and use with the AI Assitant for Optimizely

1. **Create an Azure Account**:
   - If you don't already have an Azure account, you'll need to sign up for one at [Azure's website](https://azure.microsoft.com/).
   - Once signed up, log in to your Azure portal.

2. **Create an OpenAI Resource**:
   - In the Azure portal, look for "Create a resource". 
   - Search for "OpenAI Service" and select it.
   - NEXT STEP is in Preview/BETA, you might need to fill in a form to get access, normaly it takes hours to get access.
   - Click "Create". You'll be prompted to fill in details like subscription, resource group, and region. Pick options that suit your needs.

3. **Configure the OpenAI Resource**:
   - After the resource is created, go to the resource page.
   - Here, you'll find essential details like your key and endpoint. Keep these safe, as you'll need them to interact with the OpenAI API.

4. **Set Up a Deployment**:
   - Deployments are essentially configurations of how you want to use the OpenAI models.
   - In the OpenAI resource pane, look for a section or tab related to deployments.
   - Follow the prompts to create a new deployment. You'll need to choose the model (like GPT-3, Codex, etc.), and set any specific parameters or configurations required for your use case.
  
     ![image](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/assets/9716195/d7430728-812f-41c2-959b-a4a557e3dd49)


5. **Access Keys and Endpoints**:
   - Once your deployment is ready, make sure you have your access keys and endpoints handy. These are critical for your applications to communicate with the Azure OpenAI Service.

6. **Integrate with the AI Assistant and Test Your Deployment**:
   - read more here [https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant](https://github.com/Epicweb-Optimizely/Epicweb.Optimizely.AIAssistant/blob/master/configuration.md#azure-open-ai-provider)

7. **Monitor and Manage**:
   - Azure provides tools to monitor your usage and performance. Keep an eye on these metrics to manage costs and understand your usage patterns.

Remember, Azure's documentation is quite comprehensive, so don't hesitate to refer to it for more detailed guidance. And of course, Epicweb is here to help with any specific questions or clarifications you might need along the way!
