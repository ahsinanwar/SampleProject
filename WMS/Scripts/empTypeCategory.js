$(document).ready(function () {


    var selectedItemID = document.getElementById("selectedTypeIdHidden").value;


    $('#TypeID').empty();
    var URL = '/WMS/Emp/EmpTypeList';
    //var URL = '/Emp/EmpTypeList';
    $.getJSON(URL + '/' + $('#CatID').val(), function (data) {
        var items;
        $.each(data, function (i, state) {
            if (state.Value == selectedItemID)
                items += "<option selected value='" + state.Value + "'>" + state.Text + "</option>";
            else
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#TypeID').html(items);
        $('#EmpTypeDivID').show();
    });


    $('#CatID').change(function () {
        $('#TypeID').empty();
        var URL = '/WMS/Emp/EmpTypeList';
        //var URL = '/Emp/EmpTypeList';
        $.getJSON(URL + '/' + $('#CatID').val(), function (data) {
            var items;
            $.each(data, function (i, state) {
                if (state.Value == selectedItemID)
                    items += "<option selected value='" + state.Value + "'>" + state.Text + "</option>";
                else
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#TypeID').html(items);
            $('#EmpTypeDivID').show();
        });
    });

});