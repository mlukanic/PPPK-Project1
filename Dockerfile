FROM quay.io/minio/minio

ENV MINIO_ROOT_USER=mlukanic \
    MINIO_ROOT_PASSWORD=Lookme2345!

EXPOSE 9000 9001

VOLUME /data

ENTRYPOINT ["minio"]
CMD ["server", "/data", "--console-address", ":9001"]
