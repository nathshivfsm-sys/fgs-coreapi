# FGS on EC2 (AWS dev host)

**Do not use this folder for local Docker Desktop development.**

| | **Local Docker Desktop** | **EC2 (this folder)** |
| --- | --- | --- |
| Compose file | `src/Gateway/docker-compose.yml` | `deployment/aws/ec2/docker-compose.ec2.yml` |
| Project name | `fgs-local` | `fgs-ec2` |
| Images | Built locally from Dockerfiles | Pulled from ECR (`setup-dev`, `user-dev`, `bff-dev`, …) |
| Host path | `src/Gateway/` | `/opt/fgs/` on the EC2 instance |
| Setup config | `src/SetupService/Fgs.Setup.API/appsettings.json` (mounted) | `/opt/fgs/config/setup-appsettings.json` (FgsSetup only) |
| Other secrets | Mounted appsettings + Setup `GloCredential` | `GloCredential` in RDS only |
| Gateway | HTTPS `developer.fsm.com` (local cert) | HTTP port 80 (ALB terminates TLS) |
| Deploy | `docker compose up --build` | GitHub Actions → SSM → `deploy-service.sh` |

Running `docker-compose.ec2.yml` on your laptop requires ECR login, a populated `.env`, and RDS reachable from your network — use the **Gateway** stack instead.

## EC2 quick reference

Bootstrap (once on the instance):

```bash
sudo deployment/aws/ec2/bootstrap-ec2.sh   # or copy files to /opt/fgs
```

Edit on the host:

- `/opt/fgs/config/setup-appsettings.json` — `ConnectionStrings:FgsSetup` only
- `/opt/fgs/.env` — ECR image tags, RabbitMQ password, Datadog env

Deploy one service (after CI push):

```bash
sudo /opt/fgs/deploy-service.sh setup-service dev
sudo /opt/fgs/deploy-service.sh user-service dev
sudo /opt/fgs/deploy-service.sh bff-service dev
sudo /opt/fgs/deploy-service.sh nginx dev
```

Status and logs:

```bash
cd /opt/fgs
docker compose -f docker-compose.ec2.yml ps
docker logs fgs-ec2-setup-service-1 --tail 100
docker logs fgs-ec2-user-service-1 --tail 100
docker logs fgs-ec2-bff-service-1 --tail 100
```

Full guide: [GITHUB_ACTIONS_CD_EC2.md](../manual-guide/GITHUB_ACTIONS_CD_EC2.md), [EC2_FULL_SETUP_AND_CD.md](../manual-guide/EC2_FULL_SETUP_AND_CD.md).

Example config templates (copy on EC2, do not commit secrets):

- [`.env.example`](.env.example)
- [`config/setup-appsettings.example.json`](config/setup-appsettings.example.json)
