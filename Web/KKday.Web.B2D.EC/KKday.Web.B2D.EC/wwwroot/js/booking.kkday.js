
function intKKday(){
// namespace Scope
            var Scope = Scope || {};

            var SideNav = function() {};
                 
            Scope.SideNav = new SideNav();

            //控制footer高度
            // define class Window
            var DeviceWindow = function() {};
            DeviceWindow.prototype = {
                constructor: DeviceWindow(),

                // define constant
                EXTRA_SMALL_DEVICE: 767,
                SMALL_DEVICE_TABLET: 991,
                MEDIUM_DEVICE_DESKTOP: 1199,
                LARGE_DEVICE_DESKTOP: 9999,

                getWidth: function() {
                    var width = $(window).width();
                    if (width > 0 && width < 768)
                        return this.EXTRA_SMALL_DEVICE;
                    else if (width >= 768 && width < 992)
                        return this.SMALL_DEVICE_TABLET;
                    else if (width >= 992 && width < 1200)
                        return this.MEDIUM_DEVICE_DESKTOP;
                    else if (width > 1200)
                        return this.MEDIUM_DEVICE_DESKTOP;
                    else
                        throw new RangeError("錯誤的螢幕寬度");
                },
                // 判斷使用者目前使用的裝置是否為行動裝置
                isMobile: function() {
                    var check = false;
                    (function(a) {
                        if(/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a)||/1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0,4)))
                            check = true;
                        })(navigator.userAgent||navigator.vendor||window.opera);
                    return check;
                }
            };

            Scope.DeviceWindow = new DeviceWindow();

                // define class Footer
                var Footer = function() {};
                Footer.prototype = {
                    constructor: Footer(),

                //當螢幕寬度調整時，使得 footer 四大區塊都能保持一致的高度，畫面才不會亂掉
                // footer-know-kkday, footer-for-travellers, footer-for-partner, footer-contact-us
                keepSameHeight: function() {
                    if (Scope.DeviceWindow.getWidth() == Scope.DeviceWindow.SMALL_DEVICE_TABLET) {
                        var divHeightArray = [];
                        divHeightArray.push(
                            $('#footer-know-kkday').height(),
                            $('#footer-for-travellers').height(),
                            $('#footer-for-partner').height(),
                            $('#footer-contact-us').height()
                        );
                        var maxHeight = Math.max.apply(null, divHeightArray);
                        $('#footer-know-kkday').height(maxHeight);
                        $('#footer-for-travellers').height(maxHeight);
                        $('#footer-for-partner').height(maxHeight);
                        $('#footer-contact-us').height(maxHeight);
                        //console.log('fix height: ' + maxHeight);
                    } else {
                        $('#footer-know-kkday').css('height','auto');
                        $('#footer-for-travellers').css('height','auto');
                        $('#footer-for-partner').css('height','auto');
                        $('#footer-contact-us').css('height','auto');
                    }
                },
                setTextAlign: function() {
                    if (Scope.DeviceWindow.getWidth() == Scope.DeviceWindow.EXTRA_SMALL_DEVICE) {
                        $('#footer-company-info').css('text-align', 'center');
                        $('#footer-social-button-group').css('text-align', 'center');
                        $('#contact-kkday-row').css('margin','65px 0');
                    } else {
                        $('#footer-company-info').css('text-align', 'right');
                        $('#footer-social-button-group').css('text-align', 'left');
                        $('#contact-kkday-row').css('margin','20px -15px 35px -15px');
                    }
                }
            };
            // initial object
            Scope.Footer = new Footer();

            Scope.Footer.keepSameHeight();
            Scope.Footer.setTextAlign();
            $(window).resize(function() {
                Scope.Footer.keepSameHeight();
                Scope.Footer.setTextAlign();
            });



           //Scroll to top
            $(document).on( 'scroll', function(){

                // 手機版不顯示
                if ($(window).width() < 768 ) return;

                // 電腦版
                if ($(window).scrollTop() > 100) {
                    $('.scroll-top-wrapper').addClass('show');
                } else {
                    $('.scroll-top-wrapper').removeClass('show');
                }

                //偵測頁面是否滾到底部
                var $docHeight = $(document).height();
                var $winHeight =  $(window).height();
                var $footerHeight = $('footer').height();
                var $bottomHeight = $docHeight - $winHeight - $footerHeight;
                var $asideProd = $('.booking-prod');


                if ($(window).scrollTop() < $bottomHeight) {

                    $asideProd.removeClass('active');

                }else{

                    $asideProd.addClass('active');

                }
            });

            $('.scroll-top-wrapper').on('click', scrollToTop);


            //***Booking JS Start**//

            //Datepicker

            //折扣區域＆付款區的radio選擇，展開區域效果
            $('input[name=payment],input[name=discount]').on('click',function(){

                var $this = $(this);
                var $info = $this.parent().siblings('.card-info');
                var $all = $this.parents('ul').find('.card-info');

                $all.slideUp(300);
                $info.slideDown(300);

            })

            //切換折扣/亞萬時跳出popup訊息
            $('input[name=discount]').on('click',function(){

                $('#modal_discount').modal('show');

            })

            //電話號碼Plugin
            $(".form-phone").intlTelInput({
                utilsScript: "/js/utils.js",
                preferredCountries: ["tw","kr","hk","jp","my","th","ph","sg","vn","us"],
                initialCountry: "auto",
                geoIpLookup: function(callback) {
                  $.get('https://ipinfo.io', function() {}, "jsonp").always(function(resp) {
                  var countryCode = (resp && resp.country) ? resp.country : "";
                  callback(countryCode);
                  });
                },

            });


            $('.booking-wrap aside .aside-toggle').on('click',function(){

                var $aside = $(this).parent();

                $aside.toggleClass('active');

            })

            //區塊中的下一步Button
            $('#board1 .mt-15 .btn-primary, #board2 .mt-15 .btn-primary').on('click',function(){

                var $this = $(this);
                var $must = $this.parent().parent().find('.must');
                var $href = $this.attr('data-href');
                var $board = $this.parent().parent().parent();
                var $edit = $board.find('.edit-info');

                $must.each(function(){
                    var $input = $(this).find('.form-control');
                    var $radio = $(this).find('input[type="radio"]');

                    if($input.val() == 0){
                        $(this).addClass('has-error');
                    }else{
                        $(this).removeClass('has-error');
                    }

                    // if($radio.is(":checked")){
                    //  $(this).addClass('has-error');
                    // }else{
                    //  $(this).removeClass('has-error');
                    // }

                })

                if($('.has-error').length == 0){

                    $board.removeClass('active');
                    $($href).addClass('active');

                    $this.remove();
                    $edit.addClass('show');
                }
            })

            //點按區塊右上角的編輯展開效果
            $('.title-tool').on('click',function(){
                var $this = $(this);
                var $board = $this.parent().parent();

                var className =$board.attr('class');

                if(className.indexOf('active') >= 0)
                {
                  $board.removeClass('active'); 
                }
                else
                {
                  $board.addClass('active');
                }
            })


            //Radio是否選項底下有子項目要展開時（ex:APP聯絡方式）
            $('.radio-has-son input').on('click',function(){

                var $this = $(this);
                var $parent = $this.parent().parent('.radio-has-son');
                var $input = $parent.siblings('.form-group-son');

                if($this.val() == 'yes'){
                    $input.removeClass('hide');

                }else{
                    $input.addClass('hide');

                }
            })

            //旅人資料區域中，旅客資料規格化的每一塊欄位Focus時區塊的變化
            $('.traveler-con .form-control,.traveler-con input').on('click focus',function(){

                var $this = $(this);
                var $con = $this.closest('.traveler-con');

                $('.traveler-con').removeClass('active');
                $con.addClass('active');

            })

            //包車路線 Dropdown menu （若有選項過長不適合使用<select>呈現的也可以用這個）
            $('.input-dropdown-menu li a').on('click',function(){
                var $value = $(this).text();

                $(this).parent('li').addClass('active');

                $(this).parent('li').siblings('li').removeClass('active');

                $(this).parents('.input-dropdown-menu').siblings('.input-dropdown').text($value);

                if($(this).hasClass('customize')){

                    $('#charter_group').removeClass('hidden');

                }else{

                    $('#charter_group').addClass('hidden');

                }
            })

            //自訂接駁時間
            $('#select_pickup').on('change',function(){
              var $val = $(this).val();
              var $time = $('#pickup_time');

              if($val == 'customize'){

                $time.removeClass('hidden');

              }else{

                $time.addClass('hidden');

              }
            })

            //鞋碼尺寸 (此onchange 適用單一開關事件)
            $('.select_shoe').on('change',function(){
                var $val = $(this).val();
                var $size = $(this).parent().parent().parent().find('.shoe_size');

                if($val != '0'){

                    $size.removeClass('hidden');

                }
            })

        $('#txtPayCardNum').on('paste', function(e) {
          //$(e.target).keyup(e);
            reFormatCardNumber(e);
        });

        //***Booking JS End**//

        function scrollToTop() {
            verticalOffset = typeof(verticalOffset) != 'undefined' ? verticalOffset : 0;
            element = $('body');
            offset = element.offset();
            offsetTop = offset.top;
            $('html, body').animate({scrollTop: offsetTop}, 500, 'linear');
        }


}