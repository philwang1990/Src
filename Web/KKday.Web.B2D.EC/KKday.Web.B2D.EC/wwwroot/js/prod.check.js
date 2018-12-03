//頁面初始化
function setInit() {
    $(".confirmBar").hide();
    $(".minPriceBar").show();

    if ($("#hdnIsEcSale").val() == "True") reflashPkg();
    else {
        $("#option-spy").hide();
    }
}

//daterangepicker initial
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

    $('#select-date input').daterangepicker({
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

    if ($("#hdnPreSelDate").val() != '') {
        $('#select-date input').val($("#hdnPreSelDate").val());
    } else {
        $('#select-date input').val(''); //一定要有這一行，For Daterangepicker 初始值清空
    }

    //選擇日期後套餐的 loading 效果
    $('#select-date input').on('apply.daterangepicker', function(ev, picker) {
        var dateStr = picker.startDate.format('YYYY/MM/DD');
        $("#hdnPreSelDate").val(dateStr);
        $('.replaceSelDate').html(dateStr);

        var pkgSelected = $('#hdnPkgOid').val();
        if (pkgSelected == "") {
            reflashPkg(); //重新找可用套餐
        } else {
            $('.option-booking_' + pkgSelected).show();
            BookingEvent();
        }

        $('.option-group .dot-load').show();

        setTimeout(function() {
            //套餐區塊loading效果
            $('.option-group .dot-load').hide();
        }, 1000);

        BookingCheck();
    });
}


/* ------商品明細照片集 start------ */

function photo_slideshow() {
    initCarousel();
    $('#product-photo-modal').modal('show');
}

function initCarousel() {
    $("#kk-slideshow").owlCarousel({
        items: 1,
        responsive: true,
        responsiveRefreshRate: 200,
        responsiveBaseElement: window,
        autoHeight: false,
        lazyLoad: true,
        rewind: true,
        dotsEach: true,
        dots: true,
    });
}

//商品明細上方圖片集-左
function slideshowPrev() {
    $("#kk-slideshow").trigger('prev.owl');
}

//商品明細上方圖片集-右
function slideshowNext() {
    $("#kk-slideshow").trigger('next.owl');
}

/*  ------商品明細照片集 end------  */


/* ------商品規則相關 start------ */

//套餐選擇
function chgPkgInfo(pkgOid, chk) {
    if (pkgOid == '') {
        if ($("#hdnPkgOid").val() != "" && $("#hdnPreSelDate").val() != "") {
            btnConfirm();
        } else {
            $("#hdnPkgOid").val('');
            dtInit('2');

            $("#select-date > input").focus().click();
        }

    } else {
        $("#hdnPkgOid").val(pkgOid);
        if (chk == 'showprice' && $("#hdnPreSelDate").val() != "") {
            //歸0
            $("input[id^='txtprice']").val("0");
            //把選擇數量秀出來 
            $("#divPrice_" + pkgOid).css("display", "inline");
            $("input[id^='selminus']").addClass("disabledClass");
            $("input[id^='selplus']").removeClass("disabledClass");

            $(".confirmBar").show();
            $(".minPriceBar").hide();
            $("#confirmName").text($("#hdnPkgName_" + pkgOid).val());
            $("#confirmSelDate").text($("#hdnPreSelDate").val());
            $("#confirmPrice").text("0");

            //eventtime
            if ($("#hdnPreSelDate").val() != "") {
                var test1 = $("#hdnProdOid").val();
                var eventTimes = null;
                var jqxhr = $.ajax({
                    type: "POST",
                    url: _root_path + "Product/GetEventTime/",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({
                        prodno: test1,
                        pkgno: pkgOid,
                        DateSelected: $("#hdnPreSelDate").val()
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
                                $('#hdnHasEvent_' + pkgOid).val('true');
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
            dtInit('2');

            $("#select-date > input").click();

            if (chk == "first") {
                $("#hdnShowImmediately").val("1");
            }
        }
    }
}

//刷新套餐區塊
function reflashPkg() {
    //disable div
    document.getElementById("showPkg").style.pointerEvents = "none";
    $('.option-group .dot-load').show();

    var jqxhr = $.ajax({
        type: "POST",
        url: _root_path + "Product/reflashPkg/",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            prodOid: $("#hdnProdOid").val(),
            selDate: $("#hdnPreSelDate").val()
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

            //open disable div
            document.getElementById("showPkg").style.pointerEvents = "auto";
            $('.option-group .dot-load').hide();
        },
        complete: function() {

        }
    });
}

//套餐選擇人
function PkgSelect(e) {
    var dateSelected = $("#hdnPreSelDate").val();
    var pkgNo = $(e).data('pkg-no');
    var pkgName = $(e).data('pkg-name');

    $('.confirmBar .option-title').html(pkgName);


    //event選擇清空
    $('.hdneventclass').val('');

    //按鈕效果
    $('.select-option').show();
    $('#PkgOption_' + pkgNo).hide();

    //總價清空
    $("#txtprice").text('0');
    $("#hdnPkgOid").val(pkgNo);

    if (dateSelected != '') {
        $(".option-booking").hide();
        $(".option-booking_" + pkgNo).show();
    } else {
        $(".option-booking_" + pkgNo).hide();
    }

    //選取效果
    $('.option-item').removeClass('selected');
    $('.option-item-' + pkgNo).addClass('selected');

    dtInit('2');

    BookingCheck();
}

//選擇場次
function selectEvent(eventid, eventtime, pkgId, qty) {
    $('.' + pkgId + '_eventtime_selected').html(eventtime);
    $('#hdneventQty_' + pkgId).val(qty);
    $("#hdnevent_" + pkgId).val(eventid);
    $('#EventSelect').show().html(eventtime);
    //check
    BookingCheck();
}

//取得套餐場次
function GetEvent(prodno, pkgno, date) {

    var eventTimes = null;
    var jqxhr = $.ajax({
        type: "POST",
        url: _root_path + "Product/GetEventTime/",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            prodno: prodno,
            pkgno: pkgno,
            DateSelected: date
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
                $("." + pkgno + "_eventtime_area").hide();
            } else {
                eventTimes = result.data;
                if (eventTimes.length > 0) {
                    $('#hdnHasEvent_' + pkgno).val('true');
                    $("." + pkgno + "_eventtime_area").show();
                    for (i = 0; i < eventTimes.length; i++) {
                        var timesArr = eventTimes[i].event_times.split(",");

                        $("." + pkgno + "_eventtime").empty();
                        for (i = 0; i < timesArr.length; i++) {
                            var timeInfoArr = timesArr[i].split("_");
                            var eventId = timeInfoArr[0]; //eventTime id
                            var timeStr = timeInfoArr[1]; //時間
                            var eventQty = timeInfoArr[2]; //event quantity
                            $("." + pkgno + "_eventtime").append('<li><a onClick="selectEvent(' + eventId + ',\'' + timeStr + '\',' + pkgno + ',' + eventQty + ')">' + timeStr + '</a></li>');
                        }
                    }
                } else {
                    //沒有event可選
                    //關閉booking
                    //product_index_no_event_avalible
                    getKlingon('product_index_no_event_avalible', '');
                    $('.hdneventclass').val('false');
                    $('#EventSelect').html('');
                    $('#EventSelect').hide();
                }
            }
        }
    });
}

//套餐內敘述內容
function toggleDescription(select) {
    $(select).next('div').toggle();

    if ($(select).next('div').is(":hidden")) {
        $(select).find('i').removeClass('icon-arrow-up');
        $(select).find('i').addClass('icon-arrow-down');
    } else {
        $(select).find('i').removeClass('icon-arrow-down');
        $(select).find('i').addClass('icon-arrow-up');
    }
}

//選擇數量及金額計算
function chkNum(pkgOid, qtyId, cond, priceType) {
    var thisQty = $("#" + qtyId);
    var qty = $("#hdnOrderQty_" + pkgOid).val();
    var qtyArr = qty.split(',');
    var nowQty = thisQty.val();
    var isRank = $('#hdnHasRank_' + pkgOid).val();

    var qtyindex = qtyArr.findIndex(function(item, index, array) {
        return item == nowQty;
    });
    if (nowQty == "0" && cond == "plus") {
        thisQty.val(qtyArr[0]);
    } else if (nowQty == "1" && cond == "minus") {
        thisQty.val("0");
    } else {
        if (cond == "plus") {
            thisQty.val(qtyArr[parseInt(qtyindex) + 1]);
            $("#selminus_" + priceType + "_" + pkgOid).removeClass("disabled");
        } else {
            thisQty.val(qtyArr[parseInt(qtyindex) - 1]);
            $("#selplus_" + priceType + "_" + pkgOid).removeClass("disabled");
        }
    }

    if (cond == "plus") {
        $("#selminus_" + priceType + "_" + pkgOid).removeClass("disabled");
    } else {
        $("#selplus_" + priceType + "_" + pkgOid).removeClass("disabled");
    }

    if (thisQty.val() == "0") {
        $("#selminus_" + priceType + "_" + pkgOid).addClass("disabled");
    }

    if (thisQty.val() == qtyArr[qtyArr.length - 1]) {
        $("#selplus_" + priceType + "_" + pkgOid).addClass("disabled");
    }

    //檢查人數
    if ($('#hdnHasEvent').val() === 'true' && checkEventQuantity(pkgOid)) {
        console.log($('#hdnHasEvent').val());
    }

    if (isRank === 'NORANK') {
        var checkMaxQty = checkMaxQuantity(pkgOid, $('#hdnPkgMaxNum' + pkgOid).val());
        if (!checkMaxQty) {
            $('#btnConfirm').prop('disable', true);
        } else {
            $('#btnConfirm').prop('disable', false);
        }

    } else {
        var adultQty = parseInt($("#txtprice1Qty_" + pkgOid).val()) + parseInt($("#txtprice4Qty_" + pkgOid).val());
        var checkAdultQty = checkMinAdultQuantity(pkgOid, adultQty, $('#hdnMinOverageQty_' + pkgOid).val());
        if (!checkAdultQty) {
            $('#btnConfirm').prop('disable', true);
        } else {
            $('#btnConfirm').prop('disable', false);
        }
    }

    var checkMinQty = checkMinQuantity(pkgOid, $('#hdnPkgMinNum_' + pkgOid).val());
    if (!checkMinQty) {
        $('#btnConfirm').prop('disable', true);
    } else {
        $('#btnConfirm').prop('disable', false);
    }

    if (qtyId.indexOf('price1Qty') > 0) {
        $('.rankNumberInfoAdult > .number').html(thisQty.val());
        if (parseInt(thisQty.val()) === 0) {
            $('.rankNumberInfoAdult').hide();
        } else {
            $('.rankNumberInfoAdult').show();
        }
    } else if (qtyId.indexOf('price2Qty') > 0) {
        $('.rankNumberInfoChild > .number').html(thisQty.val());
        if (parseInt(thisQty.val()) === 0) {
            $('.rankNumberInfoChild').hide();
        } else {
            $('.rankNumberInfoChild').show();
        }
    } else if (qtyId.indexOf('price3Qty') > 0) {
        $('.rankNumberInfoInfant > .number').html(thisQty.val());
        if (parseInt(thisQty.val()) === 0) {
            $('.rankNumberInfoInfant').hide();
        } else {
            $('.rankNumberInfoInfant').show();
        }
    } else if (qtyId.indexOf('price4Qty') > 0) {
        $('.rankNumberInfoElder > .number').html(thisQty.val());
        if (parseInt(thisQty.val()) === 0) {
            $('.rankNumberInfoElder').hide();
        } else {
            $('.rankNumberInfoElder').show();
        }
    }

    confirmTotalPrice(pkgOid);
    BookingCheck();
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
    $("#txtprice").text(totalPrice.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
}

function btnConfirm() {
    var pkgOid = $("#hdnPkgOid").val();
    //確認數量有被選擇

    var jqxhr = $.ajax({
        type: "POST",
        url: _root_path + "Product/confirmPkg/",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            prodOid: $("#hdnProdOid").val(),
            selDate: $("#hdnPreSelDate").val().replace("/", "").replace("/", ""),
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
        error: function(jqXHR, textStatus, errorThrown) {
            alert('要改');
        },
        success: function(result) {

            if (result.status == "OK") {} else {
                alert(result.msg);
            }
        },
        complete: function() {
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

//取得挖字
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

//檢查場次人數
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

//檢查最低成人購買數
//api 無這欄位，待補
function checkMinAdultQuantity(pkgOid, adultQuantity, minAdultQuantity) {
    if (adultQuantity < minAdultQuantity) {
        getKlingon('product_index_min_order_adult_qty_alert', minAdultQuantity);
        $('.msg-error').show();
        $('.btn-confirm-block').show();
        $('.btn-book').hide();
        return false;
    }
    $('.msg-error').hide();
    $('.btn-confirm-block').hide();
    $('.btn-book').show();
    return true;
}

//檢查最低購買數
function checkMinQuantity(pkgOid, minQuantity) {
    if (getTotalTry(pkgOid) < minQuantity) {
        getKlingon('product_index_min_order_qty_alert', minQuantity);
        $('.msg-error').show();
        $('.btn-confirm-block').show();
        $('.btn-book').hide();
        return false;
    }
    $('.msg-error').hide();
    $('.btn-confirm-block').hide();
    $('.btn-book').show();
    return true;
}

function checkMaxQuantity(pkgOid, maxQuantity) {
    if (getTotalTry(pkgOid) > maxQuantity) {
        getKlingon('product_index_max_order_qty_alert', maxQuantity);
        $('.msg-error').show();
        $('.btn-confirm-block').show();
        $('.btn-book').hide();
        return false;
    }
    $('.msg-error').hide();
    $('.btn-confirm-block').hide();
    $('.btn-book').show();
    return true;
}

function BookingEvent() {
    var dateSelected = $("#hdnPreSelDate").val();
    var pkgSelected = $("#hdnPkgOid").val();
    if (dateSelected === '') {
        dtInit('1');
        $("#select-date > input").focus().click();
        BookingNowDisplay('false');
    }
    if (dateSelected != '' && pkgSelected != '') {
        //歸0
        $("input[id^='txtprice']").val("0");
        BookingNowDisplay('true');

        var prodId = $("#hdnProdOid").val();
        var eventTimes = null;
        GetEvent(prodId, pkgSelected, $("#hdnPreSelDate").val().replace(/\//g, ""));
    }
}

function BookingNowDisplay(e) {
    if (e === 'true') {
        $(".confirmBar").show();
        $(".booking-price").show();
        $(".minPriceBar").hide();
    } else {
        $(".confirmBar").hide();
        $(".booking-price").hide();
        $(".minPriceBar").show();
    }
}

function BookingCheck() {
    var allowBook = false;
    var pkgno = $("#hdnPkgOid").val();
    //檢查日期
    if ($("#hdnPreSelDate").val() != '' &&
        pkgno != '' &&
        $('#txtprice').text() != '0' &&
        ($('#hdnHasEvent_' + pkgno).val() == 'true' && $("#hdnevent_" + pkgno).val() != '')
    ) {
        allowBook = true;
    } else if ($("#hdnPreSelDate").val() != '' &&
        pkgno != '' &&
        $('#txtprice').text() != '0' &&
        ($('#hdnHasEvent_' + pkgno).val() == 'false')
    ) {
        allowBook = true;
    } else {
        allowBook = false;
    }

    if (allowBook) {
        $('.btn-confirm-block').hide();
        $('.btn-book').show();
    } else {
        $('.btn-confirm-block').show();
        $('.btn-book').hide();
    }
}

/* ------商品規則相關 end------ */


/* ------其它 start------ */

function formatWeekDayStr(weekDays) {
    var weekDayStr = '';
    var weekDaysArr = weekDays.split(',');
    var weekDayFormat = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
    for (i = 0; i < weekDayFormat.length; i++) {
        if (weekDaysArr[i] == 'Y') {
            if (weekDayStr.length > 0) {
                weekDayStr += ', ';
            }
            weekDayStr += weekDayFormat[i];
        }
    }
}
/* ------其它 end------ */