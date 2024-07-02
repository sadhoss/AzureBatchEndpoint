# Azure ML – Deploy and integrate ml model as Batch Endpoint 

#### TL:DR  
>This repo includes an example on how to deploy and integrate the usage of an ml model with .Net. Probably the most cost effective cloud deployment strategy for your ml model! Hosted by Azure, serverless, and invokable by rest! Best part, swapping a model is as easy as swapping a URL!  

<br>  
<br>  

#### Deploy Technology | Azure ML Batch Endpoint    
Batch endpoints in Azure Machine Learning Workspace lets you process large datasets efficiently by running predictions in the background. It streamlines your workflow by automating and scheduling tasks, so you can sit back, relax, and maybe even grab a coffee while the magic happens.

<figure>
  <img src="Attachments/BatchEndpointFlow.png" alt="Data flow in Batch Endpoint">
  <figcaption>Figure 1: An illustration of information flow using Batch Endpoints (<a href="https://learn.microsoft.com/en-us/azure/machine-learning/concept-endpoints-batch?view=azureml-api-2">source</a>).</figcaption>
</figure>

Microsoft doc; [What are batch endpoints? - Azure Machine Learning | Microsoft Learn](https://learn.microsoft.com/en-us/azure/machine-learning/concept-endpoints-batch?view=azureml-api-2). BUT, as you will see later, it is somewhat lacking and **incorrect**!! (07.2024)

<br>  
<br>  
<br>  

### Highlevel Approach

**Steps in Azure Machine Learning Workspace | Deploy & Hosting**
1. Register your model in Azure Machine Learning Workspace. (Does not matter if you trained it there or you have your own custom model, you can register it in the model registry either way.)
2. Use the toolbar in your model view, and select **Deploy->Batch endpoint**. 
3. That’s "it". Now you can start using your model.

**Steps in .Net | Integration & Usage**  
1. Configure access control to Azure resources
2. Code level authentication - Azure Storage account access
3. Code level authentication - Azure Machine Learning Workspace / Batch Endpoint access.
4. Code uploading data
5. Code Invoking batch endpoint
6. Code monitoring the model prediction status 
7. Code extracting the prediction results





