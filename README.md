# RavenDB.Client
Use .net Client for the dockerized RavenDB Database

## Startup
### Docker
$ docker run -p 8080:8080 ravendb/ravendb:ubuntu-latest
https://hub.docker.com/r/ravendb/ravendb/

### .net 6
Install-Package RavenDB.Client -Version 5.3.2

### Monitoring
1. Run Docker Container
2. Open in browser (localhost:8080)
3. Agree End-User License Agreement
4. At the moment choose Unsecure-security-layout
5.  Insert docker container ip-adress (shell: docker inspect "container-name" // e.g docker inspect sleepy_swartz)
    Leave the default parameters for HTTP Port and TCP Port
6. Next, and restart server
7. Reopen localhost
