$(document).ready(function () {
	var emptyCells = [];
	$('#example thead tr').clone(true).appendTo('#example thead');
	$('#example thead tr:eq(1) th').each(function (i) {
		var title = $(this).text();
		if (title !== "") {
			$(this).html('<input type="text" placeholder="Поиск ' + title + '" />');
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

	var columnDefs = [];
	for (var i = 0; i < emptyCells.length; i++) {
		columnDefs.push({ "orderable": false, "targets": emptyCells[i]});
	}

	// DataTable
	var table = $('#example').DataTable({
		order: [],
		"language": {
			"url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Russian.json"
		},
		"columnDefs": columnDefs,
		orderCellsTop: true,
		fixedHeader: true
	});
});