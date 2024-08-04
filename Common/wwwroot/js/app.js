function jsSaveAsFile(filename, byteBase64) {
    const link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + byteBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function jsSaveAsFileX(filename, mediaType, data) {
    const link = document.createElement('a');
    link.download = filename;
    link.href = "data:" + mediaType + "," + data;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}