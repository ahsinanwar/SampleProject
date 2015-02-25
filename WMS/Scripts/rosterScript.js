//$(document).ready(function () {

//    ////var URL = '/WMS/Emp/SectionList';
//    //var URL = '/Home/TestData';
//    //$.getJSON(URL, function (data) {
//    //    console.log(data);
//    //});


//});
$(document).ready(function () {
    $("#cyclesDiv").hide();
   
    $('#RosterType').change(function () {
        var test = $(this).val();
        if (test == '2' || test == '5') {
            $("#cyclesDiv").show();
        } else {
            $("#cyclesDiv").hide();
        }
    });
    $('#dateStart').change(function () {
        var startDate = $(this).val();
        var endDate = new Date("2015-02-02");
        endDate.setDate(endDate.getDate() + 3);
        //endDate.setDate(startDate + 3);
        var dd = endDate.getDate();
        var mm = endDate.getMonth() + 1;
        var y = endDate.getFullYear();

        var someFormattedDate = dd + '/' + mm + '/' + y;
        alert(someFormattedDate);
        $('#dateEnd').val(someFormattedDate);
    });
});