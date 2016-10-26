$("#pdfExport").jqxButton();

$("#pdfExport").click(function () {
    $("#jqxgrid").jqxGrid('exportdata', 'pdf', 'jqxGrid');
});

$("#print").jqxButton();

$("#print").click(function () {
    var gridContent = $("#jqxgrid").jqxGrid('exportdata', 'html');
    var newWindow = window.open('', '', 'width=800, height=500'),
    document = newWindow.document.open(),
    pageContent =
        '<!DOCTYPE html>\n' +
        '<html>\n' +
        '<head>\n' +
        '<meta charset="utf-8" />\n' +
        '<title>jQWidgets Grid</title>\n' +
        '</head>\n' +
        '<body>\n' + gridContent + '\n</body>\n</html>';
    document.write(pageContent);
    document.close();
    newWindow.print();
});
