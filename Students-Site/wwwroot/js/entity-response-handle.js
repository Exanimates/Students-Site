$(document).ready(function () {

	$.validator.setDefaults({
		submitHandler: function (form) {
			var url = $(form).attr('action');
			var data = $(form).serialize() + '&ajax_validation=1';
			$.ajax({
				url: url,
				data: data, 
				type: 'POST',
				success: function (response) {
					swal("Отличная работа", response, "success");
				},
				error: function (data) {
					swal("Что-то не так!", data.responseText, "error");
				}
			});
			return false;
		}
	});

});