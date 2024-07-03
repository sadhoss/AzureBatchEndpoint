# Azure ML – Deploy and Integrate an ML model using Batch Endpoints 

#### TL:DR  
>This repo includes an example on how to deploy and integrate the usage of an ml model with .Net. Probably the most cost effective cloud deployment strategy for your ml model! Hosted by Azure, serverless, and invokable by rest! Best part, swapping a model is as easy as swapping a URL!  

<br>  

#### Deploy Technology | Azure ML Batch Endpoint    
Batch endpoints in Azure Machine Learning Workspace lets you process large datasets efficiently by running predictions in the background. It streamlines your workflow by automating and scheduling tasks, so you can sit back, relax, and maybe even grab a coffee while the magic happens.

<figure>
  <img src="Attachments/BatchEndpointFlow.png" alt="Data flow in Batch Endpoint">
  <figcaption>Figure 1: An illustration of information flow using Batch Endpoints (<a href="https://learn.microsoft.com/en-us/azure/machine-learning/concept-endpoints-batch?view=azureml-api-2">source</a>).</figcaption>
</figure>

<br> 

#### Information flow explenation
TODO add how data is flowing 

Microsoft doc; [What are batch endpoints? - Azure Machine Learning | Microsoft Learn](https://learn.microsoft.com/en-us/azure/machine-learning/concept-endpoints-batch?view=azureml-api-2). BUT, as you will see later, it is somewhat lacking and **incorrect**!! (07.2024)

<br>  
<br>  

---

## Repository example case: Dimond Pricing Estimation
I recently got engaged 🎉, and read a lot about the 4Cs of 💍. So, here is a diamond dataset from [Kaggle](https://www.kaggle.com/datasets/joebeachcapital/diamonds?resource=download). The data includes the 4Cs and some other parameters, including price. Let’s test Azure Batch Endpoint on estimating diamond prices based on the 4Cs.

#### Model training 
The process of splitting the data and training a model is not the focus point in this article, but the general gist is as follows:
1.	Splitt your data in 2 or 3 parts, train-test or train-validation-test.
2.	Use preferred ML approach. I think the “Auto ML” approach (insert data, get black box out) is pretty straight forward, in my case I will use the service from Azure, similar services are available elsewhere too. Steps include:
	a.	Load data to Azure 
	b.	Configure their auto ml service
	c.	Initiate the model training
	d.	Evaluate model performance
	e.	Model ready to use

<figure>
  <img src="Attachments/Model_evaluation_test_set.png" alt="Test set results">
  <figcaption>Figure 2: The results on the test(unseen) set not doing any data enhancing, using the "insert data, get black box out" approach. One might argue there is not more performance to gain.</figcaption>
</figure>

(If there is interest in the above part, maybe I will add an in-depth section for it.) 

<br>
<br>

---

## Deploy and Integrate

### Steps in Azure Machine Learning Workspace | Model Deploy & Hosting
1. Register your model in Azure Machine Learning Workspace. (Does not matter if you trained it there or you have your own custom model, you can register it in the model registry either way.)
2. Use the toolbar in your model view, and select **Deploy->Batch endpoint**. 
3. Provide environment and scoring script. When using Azure ML Workspace, this is handled for you. 
4. That’s "it". Now your model is ready to serve.

https://github.com/sadhoss/AzureBatchEndpoint/assets/16901477/11dac76a-d8bb-4cee-b238-857364cd0be9



### Steps in .Net | Integration & Usage
#### 1. Authorization required to invoke batch endpoints | Azure resources
[How authorization works | Microsoft Learn](https://learn.microsoft.com/en-us/azure/machine-learning/how-to-authenticate-batch-endpoint?view=azureml-api-2&tabs=rest#how-authorization-works) 
> To invoke a batch endpoint, the user must present a valid Microsoft Entra token representing a security principal. This principal can be a user principal or a service principal. In any case, once an endpoint is invoked, a batch deployment job is created under the identity associated with the token. The identity needs the following permissions in order to successfully create a job:  
✅ Read batch endpoints/deployments.  
✅ Create jobs in batch inference endpoints/deployment.  
✅ Create experiments/runs.  
✅ Read and write from/to data stores.  
✅ Lists datastore secrets.  

In simple terms you have to have the contributor role on the Azure ML workspace resource to invoke the batch endpoint.


#### 2. Authorization on data source | Batch Endpoint | Azure resources

When the batch endpoint is invoked you have to refrence the data you want to perform inferencing on. 
I am not sure of the limitations of batch endpoint, where it can access data from and where it cannot. 
However, as it is the Azure ML workspace (AMLW) resource that is trying to access the file, the AMLW needs to be granted read rights to the file. 
If you wish to avoid struggling with the access control for this, you can use the container stores within the azure storage 
account associated with the AMLW.    
It is important to note the AMLW configures access to the container store with its own constriants (as datastores).
Hence, if you wish to ensure the defualt authorization is enough, data that the batch endpoint is used on needs to be uploaded within the area configured for the AMLW datastores.


<div style="display: flex; justify-content: space-between;">
    <div style="flex: 1; display: flex; flex-direction: column; justify-content: center; align-items: center;">
        <img src="Attachments/AMLW_datastore.png" style="width: 90%;">
        <figcaption>Figure 3: Overview of datastores associated with the A ML Workspace.</figcaption>
    </div>
    <div style="flex: 1; display: flex; flex-direction: column; justify-content: center; align-items: center;">
        <img src="Attachments/AMLW_datastore2.png" style="width: 70%;">
        <figcaption>Figure 4: Datastores details.</figcaption>
    </div>
</div>


2. Code level authentication - Azure Storage account access
3. Code level authentication - Azure Machine Learning Workspace / Batch Endpoint access.
4. Code uploading data
5. Code Invoking batch endpoint
6. Code monitoring the model prediction status 
7. Code extracting the prediction results


