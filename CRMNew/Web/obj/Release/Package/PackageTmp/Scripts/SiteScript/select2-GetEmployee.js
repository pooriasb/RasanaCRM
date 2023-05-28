function GetEmployeeForModal() {
  $(".employee").select2({
    dropdownParent: $("#myModal"),
    templateResult: formatState,
    minimumInputLength: 1,
    templateSelection: function (data) {

      if (data.id === '') { // adjust for custom placeholder values
        return 'Custom styled placeholder text';
      }

      return data.userName;
    },
    tags: true,
    placeholder: 'کارمند . . .',
    ajax: {
      url: '/Employee/WorkFlow/GetEmployeeAjax',
      dataType: 'json',
      delay: 1000,
      processResults: function (data) {

        return {
          results: data
        };
        // Query parameters will be ?search=[term]&type=public

      }
      ,
      cache: false

    }
  });
}
function GetEmployee() {
    $(".employee").select2({
        templateResult: formatState,
        minimumInputLength: 1,
        templateSelection: function (data) {

            if (data.id === '') { // adjust for custom placeholder values
                return 'Custom styled placeholder text';
            }

            return data.userName;
        },
        tags: true,
        placeholder: 'کارمند . . .',
        ajax: {
            url: '/Employee/WorkFlow/GetEmployeeAjax',
            dataType: 'json',
            delay: 1000,
            processResults: function (data) {

                return {
                    results: data
                };
                // Query parameters will be ?search=[term]&type=public

            }
            ,
            cache: false

        }
    });
}
function formatState(state) {
    if (state.loading) {
        return "...جستجو";
    } else
    if (!(state.userName > 0)) {

        return state.userName;
    }

    var $state = $(

        '<table class="items"><tr><td>' + state.userName + '</td> <td> کد :'
        + state.id + '</td></tr><tr>' + '<td> کد پستی : ' +
        state.phone + '</td> </tr></table>'
    );
    return $state;
};