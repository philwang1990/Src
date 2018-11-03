function price_setting_init() {

    $("#mytable #checkall").click(function () {
        if ($("#mytable #checkall").is(':checked')) {
            $("#mytable input[type=checkbox]").each(function () {
                $(this).prop("checked", true);
            });
        } else {
            $("#mytable input[type=checkbox]").each(function () {
                $(this).prop("checked", false);
            });
        }
    });

    $("#add").on("show.bs.modal", function(e) {
        $("#form1").trigger('reset');
    });

    $("[data-toggle=tooltip]").tooltip();

    $('.disc_percent li').click(function(e){
       e.preventDefault();
       $("#sign_label").html($(this).text());
    });

    $(".period").daterangepicker({
        locale : { format: 'YYYY-MM-DD', separator: ' ~ ' },
        showDropdowns : true,
        alwaysShowCalendars : false
    });
 
    $(".date").daterangepicker({ 
        autoUpdateInput: false,
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

    $('#sdate').on('apply.daterangepicker', function(ev, picker) { 
        $(this).val(picker.startDate.format('YYYY-MM-DD'));
    });

    $('#edate').on('apply.daterangepicker', function(ev, picker) { 
        $(this).val(picker.startDate.format('YYYY-MM-DD'));
    });

}

function RefreshMst(page, is_recount) {
    if($('#sdate').val() != '' && $('#edate').val() != '' && $('#sdate').data('daterangepicker').startDate > $('#edate').data('daterangepicker').startDate) {
        alert('Start Date greate than End Date!'); return false;
    }

    query_params.Filter = JSON.stringify({ xid: $("#xid").val(), name: $("#name").val(), s_date:$("#sdate").val(), e_date:$("#edate").val().toUpperCase(), status:$("#status").val() });
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

////////

function mst_setting_init() {

     $(".period").daterangepicker({
        locale : { format: 'YYYY-MM-DD', separator: ' ~ ' },
        showDropdowns : true,
        alwaysShowCalendars : false
    });

    $('.disc_percent li').click(function(e){
       e.preventDefault();
       $("#sign_label").html($(this).text());
    });

    $('.sign_sel_01 li').click(function(e){
       e.preventDefault();
       $("#sign_label_01").html($(this).text());
    });

    $('.sign_sel_02 li').click(function(e){
       e.preventDefault();
       $("#sign_label_02").html($(this).text());
    });

    $("#edit_dtl").on("show.bs.modal", function(e) {
        var xid = $(e.relatedTarget).attr('data-filtre');
        $.getJSON(_root_path + "PriceSetting/GetDtl/" + xid,function(result){ 
            if(result.status== "OK") { 
                var item = JSON.parse(result.item);
                $("#edit_dtl_xid").val(item.XID);
                $("#edit_dtl_type").val(item.DISC_TYPE);
                $("#edit_dtl_cond").val(item.DISC_LIST); 
                $("#edit_dtl_desc").val(item.DISC_LIST_NAME);
                $("#edit_dtl_wb").val(item.WHITELIST);
            }
        });
    });

    $("#edit_curramt").on("show.bs.modal", function(e) {
        var xid = $(e.relatedTarget).attr('data-filtre');
        $.getJSON(_root_path + "PriceSetting/GetCurrAmt/" + xid,function(result){
             if(result.status== "OK") { 
                var item = JSON.parse(result.item);
                $("#edit_curramt_xid").val(item.XID);
                $("#edit_curamt_curr").val(item.CURRENCY);
                $("#sign_label_02").text(item.AMOUNT < 0 ? '-' : '+');
                $("#edit_curamt_price").val(Math.abs(item.AMOUNT));  
            }
        });
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
     if(!$("#add_form_dtl").valid()) {
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
    if(!$("#edit_from_dtl").valid()) {
        return;
    }

    discDtl = { xid : $("#edit_dtl_xid").val(), mst_xid: $("#mst_xid").val(), disc_type: $("#edit_dtl_type").val(), disc_list: $("#edit_dtl_cond").val(), disc_list_name: $("#edit_dtl_desc").val(), whitelist: $("#edit_dtl_wb").val() };
    console.log(JSON.stringify(discDtl));

    $.ajax({
        type: "POST",
        url: _root_path + "PriceSetting/UpdateDtl",
        contentType: "application/json",
        data: JSON.stringify(discDtl),
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            // console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        },
        success: function (result) { 
            if(result.status == "OK") {
                RefreshDtl(dtl_query_params.Paging.current_page, false);
                $('#edit_dtl').modal('hide');
            }
        }
    });
}

function RemoveDiscDtl() {
    var _xid = $("#del_dtl_xid").val();

    $.ajax({
        type: "POST",
        url: _root_path + "PriceSetting/RemoveDtl",
        contentType: "application/json",
        data: JSON.stringify({ xid: _xid }),
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
     if(!$("#add_form_curramt").valid()) { 
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
     if(!$("#edit_form_curramt").valid()) {
        return;
    }

    currAmt = { xid: $("#edit_curramt_xid").val(), mst_xid: $("#mst_xid").val(), currency: $("#edit_curamt_curr").val(), amount: $("#sign_label_02").text()+$("#edit_curamt_price").val() };
    console.log(JSON.stringify(currAmt));

    $.ajax({
        type: "POST",
        url: _root_path + "PriceSetting/UpdateCurrAmt",
        contentType: "application/json",
        data: JSON.stringify(currAmt),
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            // console.log("jqXHR => respText: " + jqXHR.responseText + ", status: " + jqXHR.status + ", readyState: " + jqXHR.readyState + ", statusText: " + jqXHR.statusText);
            console.log("textStatus: " +textStatus + ", error: " + errorThrown);
        },
        success: function (result) { 
            if(result.status == "OK") {
                RefreshCurrAmt(curamt_query_params.Paging.current_page, false);
                $('#edit_curramt').modal('hide');
            }
            else alert(result.msg);
        }
    });
}

function RemoveDiscCurrAmt() {
    var _xid = $("#del_curramt_xid").val();

    $.ajax({
        type: "POST",
        url: _root_path + "PriceSetting/RemoveCurrAmt",
        contentType: "application/json",
        data: JSON.stringify({ xid: _xid }),
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