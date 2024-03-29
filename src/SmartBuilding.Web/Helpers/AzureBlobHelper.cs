﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace SmartBuilding.Web.Helpers
{
    public class AzureBlobHelper
    {
        public string DocFolder { get; set; }
        public AzureBlobHelper()
        {
            try
            {
                Setup();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //string imageName = $"{shortid.ShortId.Generate(false, false, 5)}_{fileName}";//+ Path.GetExtension(fileName);

        }
        async void Setup()
        {
            DocFolder = AppContext.BaseDirectory + @"wwwroot/dms";
            if (!Directory.Exists(DocFolder))
            {
                Directory.CreateDirectory(DocFolder);
            }
            /*
            string storageConnection = AppConstants.BlobConn;
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

            //create a block blob 
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            //create a container CloudBlobContainer 
            cloudBlobContainer = cloudBlobClient.GetContainerReference("ngaji-online");

            //create a container if it is not already exists

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {

                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });

            }*/
        }

        public async Task<byte[]> DownloadFile(string fileName)
        {
            try
            {
                if (!string.IsNullOrEmpty(DocFolder))
                {
                    var targetFile = $"{DocFolder}/{fileName}";
                    var res = await File.ReadAllBytesAsync(targetFile);
                    return res;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            return default;
        }

        public string GetFPath(string fileName)
        {
            try
            {
                if (!string.IsNullOrEmpty(DocFolder))
                {
                    var targetFile = $"{DocFolder}/{fileName}";
                    return targetFile;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            return default;
        }
        public async Task<bool> UploadFile(string fileName, byte[] Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(DocFolder))
                {
                    var targetFile = $"{DocFolder}/{fileName}";
                    File.WriteAllBytes(targetFile, Data);
                }
                //get Blob reference

                //CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                //cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;

                //await cloudBlockBlob.UploadFromByteArrayAsync(Data, 0, Data.Length);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
