using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

const string ENDPOINT = "localhost:9000";
const string ACCESS_KEY = "mlukanic"; // tu bi trebao biti ACCESS_KEY
const string SECRET_KEY = "Lookme2345!"; // tu bi trebao biti SECRET_KEY
string bucketName = "nesto-novo";

var minioClient = new MinioClient()
                        .WithEndpoint(ENDPOINT)
                        .WithCredentials(ACCESS_KEY, SECRET_KEY)
                        .Build();


// provjeravam postoji li bucket imena bucket-test


// operacije s bucketima: napraviti bucket, izbrisati bucket,
//  izlistati objekte iz bucketa, provjeriti postoji li bucket

//operacije s objektima: prenijeti objekt (put), obrisati objekt, dohvatiti objekt,
//    provjeriti detalje objekta


bool found = false;
try
{
    found = await minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(bucketName));
}
catch (MinioException ex)
{
    Console.WriteLine($"{ex.Message}");
}


Console.WriteLine($"{(found ? "pronasao" : "nisam pronasao")}");

// upload i download objekta putem object keya

string filepath = @"C:\Users\Matija\Desktop\26286.txt";

var response = await minioClient.PutObjectAsync(
        new PutObjectArgs()
            .WithBucket(bucketName)
            .WithFileName(filepath)
            .WithContentType("application/text")
            .WithObject($"{Guid.NewGuid()}_{filepath}")
    );

Console.WriteLine($"uploaded: {response.ObjectName}");


// izlistati sve objekte iz bucketa bucket-novi

var objects = minioClient.ListObjectsEnumAsync(
        new ListObjectsArgs().WithBucket(bucketName)
    );

var firstObj = objects.ToBlockingEnumerable().ToList().ElementAt(0);

await foreach (var obj in objects) // === objects.ToBlockingEnumerable();
{
    Console.WriteLine($"{obj.Key} | {obj.Size}B | {obj.ContentType}");
}

// download prvog objekta
try
{

    var stats = await minioClient.GetObjectAsync(
            new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(firstObj.Key)
                //.WithFile("./downloaded.txt")
                .WithCallbackStream((stream) =>
                {
                    // stream.CopyTo(File.Create($"./downloaded_{firstObj.Key}"));
                    //Console.WriteLine("\nSadrzaj:");
                    //stream.CopyTo(Console.OpenStandardOutput());
                    using (var fileStream = new FileStream($"./downloaded_new.txt", FileMode.Create, FileAccess.Write))
                        stream.CopyTo(fileStream);
                })
        );
}
catch (Exception ex)
{

    Console.WriteLine(ex.Message);
}