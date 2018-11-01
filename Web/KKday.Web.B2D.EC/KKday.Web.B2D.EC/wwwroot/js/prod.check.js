function setInit() {
    $(".confirmBar").hide();
    $(".minPriceBar").show();

    if ($("#hdnIsEcSale").val() == "True") reflashPkg();

}



function setDatePicker(e) {
    var cutOfDay = '+' + $("#hdnCutOfDay").val() + 'd';

    $('#datepicker1').datepicker({
        autoclose: true,
        format: 'yyyymmdd',
        startDate: cutOfDay,

        beforeShowDay: function(d) {
            var allCanUseDate = "";
            if (e == "1") {
                allCanUseDate = $("#hdnAllCanUseDate").val();
            } else {
                if ($("#hdnPkgOid").val() != "") {
                    allCanUseDate = $("#hdnPkgDate_" + $("#hdnPkgOid").val()).val();
                } else {
                    allCanUseDate = $("#hdnAllCanUseDate").val();
                }
            }
            var allCanUseDateArr = allCanUseDate.split(',');

            var IsPermit = true;
            //allCanUseDateArr.forEach(function(element) {

            var day = d.getDay(); //
            var day = d.getDate().toString();
            if (day < 10) day = '0' + day;
            var month = (d.getMonth() + 1).toString();
            if (month < 10) month = '0' + month;
            var year = (d.getFullYear()).toString();
            var daytemp = year + month + day;

            //console.log(daytemp);

            var chk = allCanUseDateArr.find(function(item, index, array) {
                return item == daytemp;
            });

            if (chk == daytemp) {
                IsPermit = true;
            } else {
                IsPermit = false;
            }

            //}
            return IsPermit;
        }
    }).on('changeDate', function(e) {
        if ($("#select-date").val() != "") $("#hdnPreSelDate").val($("#select-date").val());
        reflashPkg(); //重新找可用套餐
        console.log("3");
        // }

    }).on('clearDate', function(e) {
        if ($("#hdnPreSelDate").val() != "") $("#select-date").val($("#hdnPreSelDate").val());
    });
}

function iniShowDate() {
    //選擇其他日期以套餐的日期為主
    //$('#datepicker1').datepicker('destroy');
    $('#select_date').data('daterangepicker').remove();
    //setDatePicker('1');
    dtInit('1');
    if ($("#select-date").val() != "") {
        var sd = formatDate($("#select-date").val());
        $("#select_date").datepicker('setDate', sd);
    }
    //$("#select_date").datepicker('show');
}

function chgPkgInfo(pkgOid, chk) {
    //var thisDate=$("#select-date").val();
    if (pkgOid == '') {
        //$("#hdnPkgOid").val("");
        $('#select-date').datepicker('destroy');
        //setDatePicker('2');
        dtInit('2');

        if ($("#select-date").val() != "") {
            var sd = formatDate($("#select-date").val());
            $("#select-date").datepicker('setDate', sd);
        }
        //$("#select-date").datepicker('show');
    } else {
        $("#hdnPkgOid").val(pkgOid);
        if (chk == 'showprice' && $("#select-date").val() != "") {
            //歸0
            $("input[id^='txtprice']").val("0");
            //把選擇數量秀出來 
            $("#divPrice_" + pkgOid).css("display", "inline");
            $("input[id^='selminus']").addClass("disabledClass");
            $("input[id^='selplus']").removeClass("disabledClass");

            $(".confirmBar").show();
            $(".minPriceBar").hide();
            $("#confirmName").text($("#hdnPkgName_" + pkgOid).val());
            $("#confirmSelDate").text($("#select-date").val());
            $("#confirmPrice").text("0");
            console.log("2");


            //eventtime
            if ($("#select-date").val() != "") {
                var test1 = $("#hdnProdOid").val();
                var eventTimes = null;
                var jqxhr = $.ajax({
                    type: "POST",
                    url: _root_path + "Product/GetEventTime/",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        prodno: test1,
                        pkgno: pkgOid,
                        DateSelected: $("#select-date").val()
                    }),
                    dataType: "json",
                    cache: false,
                    async: false,
                    //timeout: 60000,
                    error: function(jqXHR, textStatus, errorThrown) {
                        alert('要改');
                    },
                    success: function(result) {
                        if (result.errMsg === "false") {
                            $("." + pkgOid + "_eventtime_area").hide();
                        } else {
                            eventTimes = result.data;
                            if (eventTimes.length > 0) {
                                $('#hdnHasEvent').val('true');
                                $("." + pkgOid + "_eventtime_area").show();
                                for (i = 0; i < eventTimes.length; i++) {
                                    var timesArr = eventTimes[i].event_times.split(",");

                                    $("." + pkgOid + "_eventtime").empty();
                                    for (i = 0; i < timesArr.length; i++) {
                                        var timeInfoArr = timesArr[i].split("_");
                                        var eventId = timeInfoArr[0]; //eventTime id
                                        var timeStr = timeInfoArr[1]; //時間
                                        var eventQty = timeInfoArr[2]; //event quantity
                                        $("." + pkgOid + "_eventtime").append('<li><a onClick="selectEvent(' + eventId + ',\'' + timeStr + '\',' + pkgOid + ',' + eventQty + ')">' + timeStr + '</a></li>');
                                        $('#hdnevent_' + pkgOid).val(eventId);
                                    }
                                }
                            }
                        }
                    }
                });

            }
        } else //showPkgDate
        {
            //選擇其他日期以套餐的日期為主
            //$('#datepicker1').datepicker('destroy');
            //setDatePicker('2');
            //$('#select_date').data('daterangepicker').remove();
            dtInit('2');

            if ($("#select-date").val() != "") {
                var sd = formatDate($("#select-date").val());
                $("#select-date").datepicker('setDate', sd);
            }
            $("#select-date > input").click();

            if (chk == "first") {
                $("#hdnShowImmediately").val("1");
            }

        }
    }

}

function selectEvent(eventid, eventtime, pkgId, qty) {
    debugger;
    $('.' + pkgId + '_eventtime_selected').html(eventtime);
    $('#hdneventQty_' + pkgId).val(qty);
}


//刷新套餐區塊
function reflashPkg() {

    var jqxhr = $.ajax({
        type: "POST",
        url: _root_path + "Product/reflashPkg/",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            prodOid: $("#hdnProdOid").val(),
            selDate: $("#select-date").val()
        }),
        dataType: "text",
        cache: false,
        async: true,
        //timeout: 60000,
        error: function(jqXHR, textStatus, errorThrown) {
            alert('要改');
        },
        success: function(result) {
            $("#showPkg").html(result);
            if ($("#hdnPkgOid").val() != "" && $("#hdnShowImmediately").val() == "1") {
                console.log("1");
                chgPkgInfo($("#hdnPkgOid").val(), "showprice");
                $("hdnShowImmediately").val("");
            }
        },
        complete: function() {



        }
    });
}

//選擇數量及金額計算
function chkNum(pkgOid, qtyId, cond, priceType) {
    var qty = $("#hdnOrderQty_" + pkgOid).val();
    var qtyArr = qty.split(',');
    var nowQty = $("#" + qtyId).val();
    var isRank = $('#hdnHasRank_' + pkgOid).val();

    var qtyindex = qtyArr.findIndex(function(item, index, array) {
        return item == nowQty;
    });
    if (nowQty == "0" && cond == "plus") {
        $("#" + qtyId).val(qtyArr[0]);
    } else if (nowQty == "1" && cond == "minus") {
        $("#" + qtyId).val("0");
    } else {
        if (cond == "plus") {
            $("#" + qtyId).val(qtyArr[parseInt(qtyindex) + 1]);
            //$("#selminus_"+priceType+"_"+pkgOid).removeClass("disabledClass");
        } else {
            $("#" + qtyId).val(qtyArr[parseInt(qtyindex) - 1]);
            //$("#selplus_"+priceType+"_"+pkgOid).removeClass("disabledClass");
        }
    }

    if (cond == "plus") {
        $("#selminus_" + priceType + "_" + pkgOid).removeClass("disabledClass");
    } else {
        $("#selplus_" + priceType + "_" + pkgOid).removeClass("disabledClass");
    }

    if ($("#" + qtyId).val() == "0") {
        $("#selminus_" + priceType + "_" + pkgOid).addClass("disabledClass");
    }

    if ($("#" + qtyId).val() == qtyArr[qtyArr.length - 1]) {
        $("#selplus_" + priceType + "_" + pkgOid).addClass("disabledClass");
    }

    //檢查人數
    if ($('#hdnHasEvent').val() === 'true' && checkEventQuantity(pkgOid)) {
        console.log($('#hdnHasEvent').val());
    }

    if (isRank === 'NORANK') {
        var checkMaxQty = checkMaxQuantity(pkgOid, $('#hdnPkgMaxNum' + pkgOid).val());
        console.log(checkMaxQty);
    } else {
        var adultQty = parseInt($("#txtprice1Qty_" + pkgOid).val()) + parseInt($("#txtprice4Qty_" + pkgOid).val());
        var checkAdultQty = checkMinAdultQuantity(pkgOid, adultQty, $('#hdnhdnMinOverageQty_' + pkgOid).val());
        console.log(checkAdultQty);
    }

    var checkMinQty = checkMinQuantity(pkgOid, $('#hdnPkgMinNum_' + pkgOid).val());
    console.log(checkMinQty);


    confirmTotalPrice(pkgOid);
}


//confirm
function confirmTotalPrice(pkgOid) {
    //數量
    var adtPrice = 0;
    var chdPrice = 0;
    var infPrice = 0;
    var eldPrice = 0;

    $("#hdnConfirmPkgOid").val(pkgOid);
    adtPrice = parseInt($("#txtprice1Qty_" + pkgOid).val()) * parseFloat($("#hdnPrice1_" + pkgOid).val());
    if (isNaN(adtPrice)) {
        adtPrice = 0;
    }

    if ($("#hdnPrice2_" + pkgOid).val() != "") {
        chdPrice = parseInt($("#txtprice2Qty_" + pkgOid).val()) * parseFloat($("#hdnPrice2_" + pkgOid).val())
    }
    if (isNaN(chdPrice)) {
        chdPrice = 0;
    }

    if ($("#hdnPrice3_" + pkgOid).val() != "") {
        infPrice = parseInt($("#txtprice3Qty_" + pkgOid).val()) * parseFloat($("#hdnPrice3_" + pkgOid).val())
    }

    if (isNaN(infPrice)) {
        infPrice = 0;
    }

    if ($("#hdnPrice4_" + pkgOid).val() != "") {
        eldPrice = parseInt($("#txtprice4Qty_" + pkgOid).val()) * parseFloat($("#hdnPrice4_" + pkgOid).val())
    }

    if (isNaN(eldPrice)) {
        eldPrice = 0;
    }

    var totalPrice = (adtPrice + chdPrice + infPrice + eldPrice);
    $("#confirmPrice").text(totalPrice);
}


function btnConfirm() {
    var pkgOid = $("#hdnPkgOid").val();
    //確認數量有被選擇

    alert(pkgOid);

    var jqxhr = $.ajax({
        type: "POST",
        url: _root_path + "Product/confirmPkg/",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            prodOid: $("#hdnProdOid").val(),
            selDate: $("#select-date").val(),
            pkgOid: $("#hdnPkgOid").val(),
            price1Qty: chkQty($("#txtprice1Qty_" + pkgOid).val()),
            price2Qty: chkQty($("#txtprice2Qty_" + pkgOid).val()),
            price3Qty: chkQty($("#txtprice3Qty_" + pkgOid).val()),
            price4Qty: chkQty($("#txtprice4Qty_" + pkgOid).val()),
            guid: $("#hdnGuid").val(),
            pkgEvent: $("#hdnevent_" + pkgOid).val()
        }),
        dataType: "json",
        cache: false,
        async: true,
        //timeout: 60000,
        error: function(jqXHR, textStatus, errorThrown) {
            alert('要改');
        },
        success: function(result) {

            if (result.status == "OK") {
                // window.location.reload();
            } else {
                alert(result.msg);
            }
        },
        complete: function() {
            //window.open();
            //window.location.replace(_root_path +"Booking/"+ $("#hdnGuid").val());
            PostToUrl(_root_path + "Booking/", {
                guid: $("#hdnGuid").val()
            }, '', false);
        }
    });
}

function chkQty(e) {

    if (e == "" || (typeof(e) == "undefined")) {
        return "0";
    } else {
        return e;
    }
}


function formatDate(inDate) {
    if (inDate.length == 8) {
        return inDate.substr(0, 4) + "-" + inDate.substr(4, 2) + "-" + inDate.substr(6, 2);
    } else {
        return inDate;
    }
}

function rtnYear(inDate) {
    if (inDate.length == 8) {
        return inDate.substr(0, 4);
    } else {
        return inDate;
    }
}


function rtnMonth(inDate) {
    if (inDate.length == 8) {
        var m = parseInt(inDate.substr(4, 2)) - 1;
        return m;
    } else {
        return inDate;
    }
}

//Javascript 發出 POST Request
function PostToUrl(path, params, method, newWindow) {
    method = method || "post"; //預設post
    newWindow = newWindow || false; //預設不另開

    //建立一個form
    var form = document.createElement("form");
    form.setAttribute("method", method);
    form.setAttribute("action", path);
    if (newWindow) form.setAttribute("target", "_blank");

    //Form Value (用 hidden)
    for (var key in params) {
        var hiddenField = document.createElement("input");
        hiddenField.setAttribute("type", "hidden");
        hiddenField.setAttribute("name", key);
        hiddenField.setAttribute("value", params[key]);

        form.appendChild(hiddenField);
    }

    //submit
    document.body.appendChild(form);
    form.submit();

    //Sample
    //PostToUrl("/Booking/FillData", { s: "aaa" });
}

//檢查人數
function getTotalTry(pkgOid) {
    var price1Qty = parseInt($("#txtprice1Qty_" + pkgOid).val());
    if (isNaN(price1Qty)) {
        price1Qty = 0;
    }
    var price2Qty = parseInt($("#txtprice2Qty_" + pkgOid).val());
    if (isNaN(price2Qty)) {
        price2Qty = 0;
    }
    var price3Qty = parseInt($("#txtprice3Qty_" + pkgOid).val());
    if (isNaN(price3Qty)) {
        price3Qty = 0;
    }
    var price4Qty = parseInt($("#txtprice4Qty_" + pkgOid).val());
    if (isNaN(price4Qty)) {
        price4Qty = 0;
    }

    return price1Qty + price2Qty + price3Qty + price4Qty;
}

function getKlingon(key, replace) {
    $.ajax({
        type: "post",
        url: _root_path + "Product/getKlingon",
        data: {
            key: key,
            replace: replace
        },
        dataType: "json",
        success: function(result) {
            $('.msg-error').html(result.msgreturn);
            $('.msg-error').show();
        }
    })
}

function checkEventQuantity(pkgOid) {
    var eventQty = parseInt($('#hdneventQty_' + pkgOid).val());

    if (getTotalTry(pkgOid) > eventQty) {
        getKlingon('product_index_min_event_qty_alert', eventQty);
        $('.msg-error').show();
        return false;
    }
    $('.msg-error').hide();
    return true;
}

//minOrderAdultQty
//api 無這欄位，待補
function checkMinAdultQuantity(pkgOid, adultQuantity, minAdultQuantity) {
    if (bookData.hasRank === 'RANK' && adultQuantity < minAdultQuantity) {
        getKlingon('product_index_max_order_qty_alert', minAdultQuantity);
        $('.msg-error').show();
        return false;
    }
    $('.msg-error').hide();
    return true;
}

function checkMinQuantity(pkgOid, minQuantity) {
    if (getTotalTry(pkgOid) < minQuantity) {
        getKlingon('product_index_min_order_qty_alert', minQuantity);
        $('.msg-error').show();
        return false;
    }
    $('.msg-error').hide();
    return true;
}

function checkMaxQuantity(pkgOid, maxQuantity) {
    if (getTotalTry(pkgOid) > maxQuantity) {
        getKlingon('product_index_max_order_qty_alert', maxQuantity);
        $('.msg-error').show();
        return false;
    }
    $('.msg-error').hide();
    return true;
}

function dtInit(option) {
    var dateArr = [''];
    if (option === "1") {
        dateArr = ($("#hdnAllCanUseDate").val()).split(",");
    } else if (option === "2") {
        if ($("#hdnPkgOid").val() != "") {
            dateArr = ($("#hdnPkgDate_" + $("#hdnPkgOid").val()).val()).split(",");
        } else {
            dateArr = ($("#hdnAllCanUseDate").val()).split(",");
        }
    } else {

    }

    $('#select-date input').daterangepicker({g
        minDate: moment(), //今天之前的日期不可選
        singleDatePicker: true,
        showDropdowns: true, //顯示年份、月份的下拉選單
        "opens": "center",
        locale: {
            format: 'YYYY/MM/DD'
        },
        parentEl: '#select-date',
        isInvalidDate: function(date) {
            //查不到即為銷售日
            if (dateArr.indexOf(date.format('YYYY-MM-DD')) == -1) {
                return true;
            }

        }
    });

    $('#select-date input').val(''); //一定要有這一行，For Daterangepicker 初始值清空

    //選擇日期後套餐的 loading 效果
    $('#select-date input').on('apply.daterangepicker', function() {

        $('.option-group .dot-load').show();

        setTimeout(function() {
            $('.option-group .dot-load').hide();
        }, 1000);

    });
}