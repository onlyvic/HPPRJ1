using System;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace HPD.Framework.Utils.Zip
{
    public class CSharpZip
    {
        /// <summary>
        /// Nén file
        /// </summary>
        /// <param name="zipFileName">Tên file sau khi nén</param>
        /// <param name="fileToZip">Đường dẫn file cần nén</param>
        public void DoZipFile(string zipFileName, string fileToZip)
        {
            DoZipFile(zipFileName, fileToZip, string.Empty);
        }

        /// <summary>
        /// Nén file
        /// </summary>
        /// <param name="zipFileName">Tên file sau khi nén</param>
        /// <param name="fileToZip">Đường dẫn file cần nén</param>
        /// <param name="comment">Ghi chú file nén</param>
        public void DoZipFile(string zipFileName, string fileToZip, string comment)
        {
            DoZipFile(zipFileName, new string[] { fileToZip }, comment);
        }

        /// <summary>
        /// Nén nhiều file
        /// </summary>
        /// <param name="zipFileName">Tên file sau khi nén</param>
        /// <param name="fileToZip">Mảng đường dẫn file cần nén</param>
        /// <param name="comment">Ghi chú file nén</param>
        /// ///
        public void DoZipFile(string zipFileName, string[] fileToZip, string comment)
        {
            if (string.IsNullOrEmpty(zipFileName) || fileToZip == null)
                throw new ArgumentException("zipFileName or fileToZip is null");

            if (fileToZip.Length == 0)
                return;

            FileSystemInfo[] arrFsi = new FileSystemInfo[fileToZip.Length];
            for (int i = 0; i < fileToZip.Length; i++)
            {
                arrFsi[i] = new FileInfo(fileToZip[i]);
            }

            DoZipFile(zipFileName, arrFsi, comment);
        }

        /// <summary>
        /// Nén file
        /// </summary>
        /// <param name="zipFileName">Tên file sau khi nén</param>
        /// <param name="fileToZip">Mảng file cần nén</param>
        /// <param name="comment">Ghi chú file nén</param>
        public void DoZipFile(string zipFileName, FileSystemInfo[] fileToZip, string comment)
        {

            ZipFile zip = ZipFile.Create(zipFileName);

            try
            {
                zip.BeginUpdate();
                GetFilesToZip(fileToZip, zip);
                if (!string.IsNullOrEmpty(comment))
                    zip.SetComment(comment);
                zip.CommitUpdate();
                //zip.TestArchive(true);
                zip.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                zip.Close();
            }
        }

        /// <summary>
        /// Nén đường dẫn
        /// </summary>
        /// <param name="zipFileName">Tên file sau khi nén</param>
        /// <param name="directory">Đường dẫn cần nén</param>
        public void DoZipDirectory(string zipFileName, string directory)
        {
            DoZipDirectory(zipFileName, new DirectoryInfo(directory));
        }

        /// <summary>
        /// Nén đường dẫn
        /// </summary>
        /// <param name="zipFileName">Tên file sau khi nén</param>
        /// <param name="directory">Đường dẫn cần nén</param>
        public void DoZipDirectory(string zipFileName, DirectoryInfo pathToZip)
        {
            DoZipDirectory(zipFileName, (FileSystemInfo)pathToZip);
        }

        /// <summary>
        /// Nén đường dẫn
        /// </summary>
        /// <param name="zipFileName">Tên file sau khi nén</param>
        /// <param name="directory">Đường dẫn cần nén</param>
        private void DoZipDirectory(string zipFileName, FileSystemInfo pathToZip)
        {
            DoZipFileDirectory(zipFileName, new FileSystemInfo[] { pathToZip });
        }

        private void DoZipFileDirectory(string zipFileName, FileSystemInfo[] pathToZip)
        {
            ZipFile zip = ZipFile.Create(zipFileName);

            try
            {
                zip.BeginUpdate();
                GetFilesToZip(pathToZip, zip);
                zip.CommitUpdate();
                //zip.TestArchive(true);
                zip.Close();
            }
            catch
            {
                zip.Close();
                throw;
            }
        }

        private void GetFilesToZip(FileSystemInfo[] filesToZip, ZipFile zip)
        {
            if (filesToZip == null || zip == null)
                return;
            foreach (FileSystemInfo fsi in filesToZip)
            {
                if (fsi is DirectoryInfo)
                {
                    DirectoryInfo di = fsi as DirectoryInfo;
                    GetFilesToZip(di.GetFileSystemInfos(), zip);
                }
                else
                {
                    zip.Add(fsi.FullName);
                }
            }
        }
    }
}
