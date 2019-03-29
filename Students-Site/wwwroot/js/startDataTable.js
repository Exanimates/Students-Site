$(document).ready(function () {
    startDataTable($("table[name='dataTable']"));
});

function startDataTable(table) {
    var emptyCells = [];

    $(table).find("thead tr").clone(true).appendTo($(table).find("thead"));
    $(table).find("thead tr:eq(1) th").each(function (i) {
        var title = $(this).text();
        if (title !== "") {
            $(this).html('<input type="text" placeholder="Поиск" />');
        } else {
            emptyCells.push(i);
        }

        $('input', this).on('keyup change', function () {
            if (table.column(i).search() !== this.value) {
                table
                    .column(i)
                    .search(this.value)
                    .draw();
            }
        });
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