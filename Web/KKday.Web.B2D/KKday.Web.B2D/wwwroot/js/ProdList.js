
$(function () {
    //Date range
    $('#daterange').daterangepicker({
        autoUpdateInput:false,  //input 預設沒有值
        minDate:moment(),       //今天之前的日期不可選
    });

    //點擊 Apply 後將值帶入 input
    $('#daterange').on('apply.daterangepicker', function(ev, picker) {
        $(this).val(picker.startDate.format('YYYY/MM/DD') + '-' + picker.endDate.format('YYYY/MM/DD'));
        $(this).siblings('.clear-input').show();
        $("#form2").submit();
    });

    $(document).on('click','.filter-mobile',function(){

      // 顯示選單
      $(".product-filiter").addClass("slideUp");

      // 設定不是電腦查詢
      is_pc_search=false;

    });

    //Open sub-category dropdown on Hover
    $('.main-category .dropdown').hover(function(){
      if($(window).width() > 991){
        $(this).find($('.dropdown-toggle')).trigger('click');
      };
    });



    $('.main-category li.dropdown').hover(
        function(){
            $(this).addClass('open');
            $(this).children('.sub-category').show();
        },
        function(){
            $(this).removeClass('open');
            $(this).children('.sub-category').hide();
        }
    );

    //關閉選單
    var $close = $('#close-filter, .apply-filter, .main-category li a, .sub-category li')

        $($close).on('click',function(e){
             $(".product-filiter").removeClass("slideUp");
        });

    //Checkbox 效果
    $('.checkbox-group .checkbox').on('click', function(e){
        var $this = $(this);
        var $icon = $this.find('i');
        var $on = 'fa-check-square';
        var $off = 'fa-square-o';

        e.preventDefault();

        if($this.hasClass('checked')){
            //Set Unchecked
            $icon.removeClass($on).addClass($off);
            $this.removeClass('checked');

            $("#form2").submit();

        }else if($this.hasClass('disabled')){
            //如果Disabled
            return false;

        }else if(!$this.hasClass('checked') || !$this.hasClass('disabled')){
            //Set Checked
            $icon.addClass($on).removeClass($off);
            $this.addClass('checked');

            $("#form2").submit();
        }
    });

    //Collapsable filter-box
    $('.filter-box h4').on('click',function(){

        $(this).toggleClass('collapsed');
        $(this).siblings('.filter-list').collapse('toggle');

    });

    //Scroll 滾動至最上層
    $(document).on( 'scroll', function(){

        // 手機版不顯示
        if ($(window).width() < 768 ) return;

        // 電腦版
        if ($(window).scrollTop() > 100) {
            $('.scroll-top-wrapper').addClass('show');
        } else {
            $('.scroll-top-wrapper').removeClass('show');
        }

        var $sheetHeight = $('.header-sheet').outerHeight();
        if($(window).scrollTop() > $sheetHeight){
            $('body').addClass('header-fix');
            $('header').addClass('fix');
        } else{
            $('body').removeClass('header-fix');
            $('header').removeClass('fix');
        }

    });

    //觸發scrollToTop
    $('.scroll-top-wrapper').on('click', scrollToTop);



    //Layout-CitySearch
    $("#sub2").click(function(){
        $("#requery_key").val($("#key").val());
        $("#form3").submit();
    });

})

function qSearch(citykey)
{
    $("#citykey").val(citykey);
    //alert($("#city_key").val());
    $("#form2").submit();
}

//觸發分頁
function ReQuery(page)
{
    $("#pg_idx").val(page);  
    $("#form2").submit();
}
//主分類查詢
function QueryMainKey(catmain_key)
{
    $("#catmain_key").val(catmain_key);
    $("#form2").submit();
}
//次分類查詢
function QuerySubKey(catsub_key)
{
    $("#catsub_key").val(catsub_key);
    $("#form2").submit();
}
//Dynamic input
function Submit()
{
    //duration
    $('#duration li label.checked span').each(function(){
        $('<input>').attr({
            type: 'hidden',
            value: $(this).attr('value'),
            name: 'duration'
        }).appendTo('#form2');
    });

    //guidelang
    $('#guidelang li label.checked span').each(function(){
        $('<input>').attr({
            type: 'hidden',
            value: $(this).attr('value'),
            name: 'guidelang'
        }).appendTo('#form2');
    });
}
//Scroll 滾動至最上層
function scrollToTop() {
    verticalOffset = typeof(verticalOffset) != 'undefined' ? verticalOffset : 0;
    element = $('body');
    offset = element.offset();
    offsetTop = offset.top;
    $('html, body').animate({scrollTop: offsetTop}, 500, 'linear');
}

//Layout-CitySearch
function quicksearch(key1)
{
    $("#requery_key").val(key1);
    $("#form3").submit();
}
