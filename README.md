## AB Search

Search audiobooks by title.

### Usage
#### Instructions for local bootstrap
```bash
docker-compose up
```
It will deliver next services:
 - Elasticsearch
 - REST API Server
 - Scheduler for uploading books meta into db.

#### Endpoints

 - [http://localhost/swagger/index.html](http://localhost/swagger/index.html) - OpenAPI renderer with self-explained APIs
 - [http://localhost:5000/hangfire](http://localhost:5000/hangfire) - Hangfire Scheduler dashboard

### Development

In development mode, you can use only`docker-compose up am_elasticsearch` for setting up database.

### TBD

 - Security
 - Scaling
 - K8s configs.
 - Stemmer for Russian language. 