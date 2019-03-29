$(document).ready(function () {

	$.validator.setDefaults({
		submitHandler: function (form) {
			var url = $(form).attr('action');
			var data = $(form).serialize() + '&ajax_validation=1';
			$.ajax({
				url: url,
				data: data,
				type: 'POST',
				success: function () {
					window.location.href = "/";
				},
				error: function (data) {
                    swal("Что-то не так!", data.responseJSON.Message, "error");
				}
			});
			return false;
		}
	});

});