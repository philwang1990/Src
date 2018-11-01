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

function RefreshMst(page, is_recount) {

    query_params.Filter = ""; // JSON.stringify({ name:$("#name").val(), country:$("#country").val().toUpperCase(), status:$("#status").val() });
    query_params.Sorting = ""; // $("#sorting").val();
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
    if(!$("#add_form").valid()) {
        return;
    }

    var discMst = {
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


function UpdateDiscountMst()
{ 
    if(!$("#edit_form").valid()) {
        return;
    }

    var discMst = {
        xid: $("#mst_xid").val(),
        disc_name: $("#disc_name").val(), 
        disc_type: $("#disc_type").val(),   
        disc_percent: $("#sign_label").text() + $("#disc_percent").val(), 
        s_date: $(".period").data('daterangepicker').startDate.format('YYYY-MM-DD'), 
        e_date: $(".period").data('daterangepicker').endDate.format('YYYY-MM-DD'), 
        status: $("#disc_type").val(),
        rule_status: $("input[name=rule_status]:checked").val()
    };

    console.log(JSON.stringify(discMst));
 
    $.ajax({
        type: "POST",
        url: _root_path + "PriceSetting/UpdateMst",
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
                alert("Update OK!");

                var newForm = $('<form>', {
                    'action': _root_path + "PriceSetting/",
                    'target': '_self',
                    'method': 'post'
                }).append(jQuery('<input>', {
                    'name': 'query',
                    'value': /query=([^&#=]*)/.exec(window.location.search)[1],
                    'type': 'hidden'
                }));
                newForm.submit();
            }
            else alert(result.msg);
        }
    });
 
}

function RemoveDiscountMst()
{
    var xid= $('#del_mst_xid').val();

    $.ajax({
        type: "POST",
        url: _root_path + "PriceSetting/RemoveMst",
        contentType: "application/json",
        data: JSON.stringify({ xid : xid}),
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        }, 
        success: function (result) { 
            if(result.status == "OK") {
                window.location.reload();
            }
            else alert(result.msg);
        }
    });
}

/////////////

function RefreshDtl(page, is_recount) {

    dtl_query_params.Filter = ""; // JSON.stringify({ name:$("#name").val(), country:$("#country").val().toUpperCase(), status:$("#status").val() });
    dtl_query_params.Sorting = ""; // $("#sorting").val();
    if (page!==undefined) dtl_query_params.Paging.current_page = page;
    dtl_query_params.RecountFlag= is_recount;
    //console.log("Params: " + JSON.stringify(query_params));

    $.ajax({
        type: "POST",
        url: _root_path + "PriceSetting/RefreshDtl?id=" + $("#mst_xid").val(),
        contentType: "application/json",
        data: JSON.stringify(dtl_query_params),
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            // console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        },
        success: function (result) { 
            if(result.status == "OK") {
                $("#dtl_panel").html(result.content);
                dtl_query_params = JSON.parse(result.query_params); 
            }
        }
    });
}

function InsertDiscDtl(mst_xid) {
     console.log("InsertDiscDtl");
     if(!$("#add_dtl_form").valid()) {
        return;
    }

    discDtl = { mst_xid: $("#mst_xid").val(), disc_type: $("#add_dtl_type").val(), disc_list: $("#add_dtl_cond").val(), disc_list_name: $("#add_dtl_desc").val(), whitelist: $("#add_dtl_wb").val() };
    console.log(JSON.stringify(discDtl));

    $.ajax({
        type: "POST",
        url: _root_path + "PriceSetting/InsertDtl",
        contentType: "application/json",
        data: JSON.stringify(discDtl),
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            // console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        },
        success: function (result) { 
            if(result.status == "OK") {
                window.location.reload();   
            }
        }
    });
}

function UpdateDiscDtl() {
     if(!$("#edit_dtl_form").valid()) {
        return;
    }
}

function RemoveDiscDtl() {
    var xid = $("#del_dtl_xid").val();
}


/////////////

function RefreshCurrAmt(page, is_recount) {

    curamt_query_params.Filter = ""; // JSON.stringify({ name:$("#name").val(), country:$("#country").val().toUpperCase(), status:$("#status").val() });
    curamt_query_params.Sorting = ""; // $("#sorting").val();
    if (page!==undefined) curamt_query_params.Paging.current_page = page;
    curamt_query_params.RecountFlag= is_recount;
    //console.log("Params: " + JSON.stringify(curamt_query_params));

    $.ajax({
        type: "POST",
        url: _root_path + "PriceSetting/RefreshCurrAmt?id=" + $("#mst_xid").val(),
        contentType: "application/json",
        data: JSON.stringify(curamt_query_params),
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            // console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        },
        success: function (result) {
            // console.log("result: " + JSON.stringify(result));
            if(result.status == "OK") {
                $("#curamt_panel").html(result.content);
                curamt_query_params = JSON.parse(result.query_params); 
            }
        }
    });
}

function InsertDiscCurrAmt() {
     if(!$("#add_curramt_form").valid()) { 
        return;
    }

    currAmt = { mst_xid: $("#mst_xid").val(), currency: $("#add_curamt_curr").val(), amount: $("#sign_label_01").text()+$("#add_curamt_price").val() };
    console.log(JSON.stringify(currAmt));

    $.ajax({
        type: "POST",
        url: _root_path + "PriceSetting/InsertCurrAmt",
        contentType: "application/json",
        data: JSON.stringify(currAmt),
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            // console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        },
        success: function (result) { 
            if(result.status == "OK") {
                window.location.reload();   
            }
            else alert(result.msg);
        }
    });
}

function UpdateDiscCurrAmt() {
     if(!$("#edit_curramt_form").valid()) {
        return;
    }
}

function RemoveDiscCurrAmt() {
    var xid = $("#del_curramt_xid").val();
}