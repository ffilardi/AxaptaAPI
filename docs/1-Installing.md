### Pre-requirements

* Visual Studio 2015/Visual Studio Community;
* Admin access to an instance of AX 2012 to setup the AIF Inbound ports;
* Admin access to the VM running the AOS to open the firewall ports (if required);
* Azure subscription for cloud deployment (if required).


### Deployment

The deployment can be done on a local web server (IIS/IIS Express) or on cloud (Azure web application). Others solutions (e.g. Apache, AWS) are technically possible but not tested.

**Note:** If you deploy on Azure you can point your API to any AOS running on a VM or access your on-premises AOS via VPN or enabling the Hybrid Connection between your web application and the AOS server. More instructions here:

* [Hybrid Connections overview](https://azure.microsoft.com/en-us/documentation/articles/integration-hybrid-connection-overview/)  
* [Access on-premises resources using hybrid connections in Azure App Service](https://azure.microsoft.com/en-us/documentation/articles/web-sites-hybrid-connection-get-started/)  
* [Create a resource manager VNet with a Site-to-Site VPN connection using the Azure Portal](https://azure.microsoft.com/en-us/documentation/articles/vpn-gateway-howto-site-to-site-resource-manager-portal/)
