# IPService
## For what?
If you have some clients in the internet (like server), you have to know the IP. With this service your server's send you an request and logs the IP.

## Setup server
1.  Start the server on your server and configure the config.txt
2. You ca use an Reverse Proxy to add SSL
3. Create a new API token via `` ./IPService -newKey NAME``
4. Start the server after you added your servers

**On Windows run as admin**

## Setup the client
1. Add a auto startup service with the args
`` ./IPServiceClient TOKEN KEY IP/DOMAIN INTERVAl``
__interval in minutes__

