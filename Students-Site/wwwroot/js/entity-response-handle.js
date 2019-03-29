$(document).ready(function () {

	$.validator.setDefaults({
        submitHandler: function (form) {
            var jqueryForm = $(form);

            var url = jqueryForm.attr('action');
            var data = jqueryForm.serialize() + '&ajax_validation=1';
			$.ajax({
				url: url,
				data: data, 
				type: 'POST',
				success: function (response) {
					swal("Отличная работа", response, "success");
				},
				error: function (data) {
					swal("Что-то не так!", data.responseJSON.Message, "error");
				}
			});
			return false;
		}
	});

});