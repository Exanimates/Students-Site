$(document).ready(function () {
	$('#example thead tr').clone(true).appendTo('#example thead');
	$('#example thead tr:eq(1) th').each(function (i) {
		var title = $(this).text();
		if (title !== "") {
			$(this).html('<input type="text" placeholder="Поиск ' + title + '" />');
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

	// DataTable
	var table = $('#example').DataTable({
		"language": {
			"url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Russian.json"
		},
		"columnDefs": [
			{ "orderable": false, "targets": $('#example tfoot th').length - 1 },
			{ "orderable": false, "targets": $('#example tfoot th').length - 2 }
		],
		orderCellsTop: true,
		fixedHeader: true
	});
});