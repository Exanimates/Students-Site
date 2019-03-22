$(document).ready(function () {
	// Setup - add a text input to each footer cell
	$('#example tfoot th').each(function () {
		var title = $(this).text();
		if (title !== "") {
			$(this).html('<input type="text" class="form-control form-control-sm" placeholder="Поиск"/>');
		}
	});

	// DataTable
	var table = $('#example').DataTable({
		"language": {
			"url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Russian.json"
		}
	});

	// Apply the search
	table.columns().every(function () {
		var that = this;

		$('input', this.footer()).on('keyup change', function () {
			if (that.search() !== this.value) {
				that
					.search(this.value)
					.draw();
			}
		});
	});
});