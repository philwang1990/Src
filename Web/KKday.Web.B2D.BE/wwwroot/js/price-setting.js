function price_setting_init() {
    moment.locale('zh-tw');

    $(".period").daterangepicker({
        locale : { format: 'YYYY-MM-DD', separator: ' ~ ' },
        showDropdowns : true,
        alwaysShowCalendars : false
    });
 
    $(".date").daterangepicker({
       locale : { format: 'YYYY-MM-DD', separator: ' ~ ' },
       showDropdowns : true,
       singleDatePicker : true
       /*
       isInvalidDate: function(date) {
            var disabled_start = moment('2018-10-31', 'YYYY-MM-DD');
            var disabled_end = moment('2018-11-07', 'YYYY-MM-DD');
            return date.isAfter(disabled_start) && date.isBefore(disabled_end);
       } */
     });

}

function Refresh(page, is_recount) {

    query_params.Filter = JSON.stringify({ name:$("#name").val(), country:$("#country").val().toUpperCase(), status:$("#status").val() });
    query_params.Sorting = $("#sorting").val();
    if (page!==undefined) query_params.Paging.current_page = page;
    query_params.RecountFlag= is_recount;
    //console.log("Params: " + JSON.stringify(query_params));

    $.ajax({
        type: "POST",
        url: _root_path + "PriceSetting/Refresh",
        contentType: "application/json",
        data: JSON.stringify(query_params),
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            // console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        },
        success: function (result) {
            // console.log("result: " + JSON.stringify(result));
            if(result.status == "OK") {
                $("#list_panel").html(result.content);
                query_params = JSON.parse(result.query_params); 
            }
        }
    });
}

function InsertDiscountMst()
{
    console.log("call InsertDiscountMst.....");

    if(!$("#add_form").valid()) {
        return;
    }

    var discMst = { 
        disc_no: $("#disc_no").val(), 
        disc_name: $("#disc_name").val(), 
        disc_type: $("#disc_type").val(),   
        disc_percent: $("#sign_label").text() + $("#disc_percent").val(), 
        s_date: $(".period").data('daterangepicker').startDate.format('YYYY-MM-DD'), 
        e_date: $(".period").data('daterangepicker').endDate.format('YYYY-MM-DD'), 
        status: $("#disc_type").val(),
        rule_status: $("input[name=rule_status]").val()
    };

    console.log(JSON.stringify(discMst));
 
    $.ajax({
        type: "POST",
        url: _root_path + "PriceSetting/InsertMst",
        contentType: "application/json",
        data: JSON.stringify(discMst),
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        }, 
        success: function (result) {
            // console.log("result: " + JSON.stringify(result));
            if(result.status == "OK") {
                window.location.reload();
            }
            else alert(result.msg);
        }
    });  
 
}

function DiscountMstEdit(id) {
    var query = encodeURI(JSON.stringify(query_params));
    window.location.href = _root_path + "PriceSetting/MstProfile/" + id + "?query=" + query;
} 

