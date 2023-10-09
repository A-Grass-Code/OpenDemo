/**
 * 用 a 标签下载文件
 * @param {*} downloadUrl 下载 URL
 * @param {*} fileName 文件名
 */
const aTagDownloadFile = (downloadUrl, fileName) => {
    const link = document.createElement('a');
    link.setAttribute('visibility', 'hidden');
    link.download = fileName;
    link.href = downloadUrl;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

export default aTagDownloadFile;