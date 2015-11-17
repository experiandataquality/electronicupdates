//-----------------------------------------------------------------------
// <copyright file="LocalFileStoreTests.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.IO;
using Xunit;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    public static class LocalFileStoreTests
    {
        [Fact]
        public static void LocalFileStore_Registers_Files()
        {
            // Arrange
            string dataFileName = "LocalFileStore.Test.eu";

            string hash = "7039d49e15fd4e164e2c07fe76fd61a2";
            string path = "MyFile.txt";

            try
            {
                if (File.Exists(dataFileName))
                {
                    File.Delete(dataFileName);
                }

                using (LocalFileStore target = new LocalFileStore())
                {
                    // Act
                    Assert.False(target.ContainsFile(hash));
                    Assert.False(target.ContainsFile(hash.ToUpperInvariant()));
                }

                using (LocalFileStore target = new LocalFileStore(dataFileName))
                {
                    // Act
                    Assert.False(target.ContainsFile(hash));
                    target.RegisterFile(hash, path);
                }

                using (LocalFileStore target = new LocalFileStore(dataFileName))
                {
                    // Assert
                    Assert.True(target.ContainsFile(hash.ToUpperInvariant()));

                    string storedPath;
                    bool result = target.TryGetFilePath(hash, out storedPath);

                    Assert.True(result);
                    Assert.Equal(Path.GetFullPath(path), storedPath);
                }
            }
            finally
            {
                File.Delete(dataFileName);
            }
        }
    }
}
