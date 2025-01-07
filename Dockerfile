docker run `
   -p 9000:9000 `
   -p 9001:9001 `
   --name minio1 `
   -v /d/minio/data:/data `
   -e "MINIO_ROOT_USER=mlukanic" `
   -e "MINIO_ROOT_PASSWORD=Lookme2345!" `
   quay.io/minio/minio server /data --console-address ":9001"
