using System;
using System.IO;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Utilities
{
    public class TempDirectory : IDisposable
    {
        public string Path { get; }

        public TempDirectory(string path)
        {
            Path = System.IO.Path.Join(System.IO.Path.GetTempPath(), path);
            Directory.CreateDirectory(Path);
        }

        private void ReleaseUnmanagedResources()
        {
            Directory.Delete(Path, true);
        }

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TempDirectory()
        {
            Dispose(false);
        }
    }
}
