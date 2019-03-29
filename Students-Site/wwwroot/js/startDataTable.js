$(document).ready(function () {
    startDataTable($("table[name='dataTable']"));
});

function startDataTable(table) {
    var emptyCells = [];

    $(table).find("thead tr th").each(function (i) {
        var title = $(this).text();
        if (title === "") {
            emptyCells.push(i);
        }
    });

    var columnDef = [];
    for (var i = 0; i < emptyCells.length; i++) {
        columnDef.push({ "orderable": false, "targets": emptyCells[i] });
    }

    $(table).DataTable({
        order: [],
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Russian.json"
        },
        "columnDefs": columnDef,
        orderCellsTop: true,
        fixedHeader: true
    });
}