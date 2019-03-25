$(document).ready(function () {
	var emptyCellCount = 0;
	$('#example thead tr').clone(true).appendTo('#example thead');
	$('#example thead tr:eq(1) th').each(function (i) {
		var title = $(this).text();
		if (title !== "") {
			$(this).html('<input type="text" placeholder="Поиск ' + title + '" />');
		} else {
			emptyCellCount++;
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

	var columnDefs = [];
	for (var i = 1; i <= emptyCellCount; i++) {
		columnDefs.push({ "orderable": false, "targets": $('#example tfoot th').length - i });
	}

	// DataTable
	var table = $('#example').DataTable({
		"language": {
			"url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Russian.json"
		},
		"columnDefs": columnDefs,
		orderCellsTop: true,
		fixedHeader: true
	});
});