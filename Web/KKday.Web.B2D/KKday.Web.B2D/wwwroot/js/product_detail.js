
        $(function(){

            var vm = new Vue({
                el: '#productDetailApp',
            });
           
        // 不管有沒有登入都會用到的script
            $('#header-main-sidenav-button').on('click', function(e) {
                e.stopPropagation();
                Scope.SideNav.open('main-sidenav');
            });
            $('#switch-currency-btn').click(function(e) {
                e.stopPropagation();
                Scope.SideNav.close();
                Scope.SideNav.open('currency-sidenav');
            });
            $('#switch-language-btn').click(function(e) {
                e.stopPropagation();
                Scope.SideNav.close();
                Scope.SideNav.open('language-sidenav');
            });

            // namespace Scope
            var Scope = Scope || {};

            // Global Event Listener
            // 按下頁面上任何地方，都會關掉SideNav
            // 可以使用 event.stopPropagation() 設定要哪些 event 不觸發下面這個 function
            $('html').click(function() {
                Scope.SideNav.close();
            });

            $('#close-main-sidenav').click(function(){
                 Scope.SideNav.close('main-sidenav');
            });

            $('#select-language').click(function(e) {
                e.stopPropagation();
                Scope.SideNav.close();
                Scope.SideNav.open('language-sidenav');
            });

            $('#currency-return-main-sidenav').on('click', function(e){
                e.stopPropagation();
                Scope.SideNav.sub.close('currency-sidenav');
            });

            $('#language-return-main-sidenav').on('click', function(e){
                e.stopPropagation();
                Scope.SideNav.sub.close('language-sidenav');
            });

            $('#currency-close-main-sidenav').on('click', function(e){
                e.stopPropagation();
                Scope.SideNav.sub.deepClose('currency-sidenav');
            });

            $('#language-close-main-sidenav').on('click', function(e){
                e.stopPropagation();
                Scope.SideNav.sub.deepClose('language-sidenav');
            });

            $('#select-currency').click(function(e) {
                e.stopPropagation();
                Scope.SideNav.close();
                Scope.SideNav.open('currency-sidenav');
            });
            $('#language-list li.has-son').on('click', function(e) {
                var $this = $(this);
                var $siblings = $this.siblings('li.has-son');

                $siblings.removeClass('open');
                $this.toggleClass('open');

                e.stopPropagation();
            });

            $('#language-list ul.nav-menu-son li').on('click', function(e) {
                var $this = $(this);
                var $others = $('ul.nav-menu-son li');

                Scope.SideNav.close();
                $others.removeClass('active');
                $this.addClass('active');

                e.stopPropagation();
            });

            function redirectMaker(element, event) {
                return {
                    setListener: function() {
                        element.on(event, function() {
                            window.location.replace($(this).attr('href'));
                        });
                    }
                };
            }

            var logoutRedirect = redirectMaker($('#logout'), 'click');
            var accountSettingRedirect = redirectMaker($('#account-setting'), 'click');
            var checkWishListRedirect = redirectMaker($('#check-wishlist'), 'click');
            var checkNoticeListRedirect = redirectMaker($('#check-noticelist'), 'click');
            var checkMessageRedirect = redirectMaker($('#check-messages'), 'click');
            var checkOrderRedirect = redirectMaker($('#check-order'), 'click');
            var checkCouponRedirect = redirectMaker($('#check-coupons'), 'click');

            logoutRedirect.setListener();
            accountSettingRedirect.setListener();
            checkWishListRedirect.setListener();
            checkNoticeListRedirect.setListener();
            checkMessageRedirect.setListener();
            checkOrderRedirect.setListener();
            checkCouponRedirect.setListener();

            var SideNav = function() {};

            SideNav.prototype = {
                open: function(id) {
                    var nav = $('#' + id);
                    nav.addClass('open');
                },
                close: function(id) {
                    id = id || null;
                    // 如沒有傳入id 就把class是 kk-sidenav 的側邊欄都關閉
                    var nav = (null === id) ? $('.kk-sidenav') : $('#' + id);
                    nav.removeClass('open');
                },
                sub: {
                    open: function(id) {
                        var nav = $('#' + id);
                        nav.addClass('open');
                    },
                    close: function(id) {
                        Scope.SideNav.close(id);
                        Scope.SideNav.open('main-sidenav');
                    },
                    deepClose: function(id) {
                        Scope.SideNav.close(id);
                    }
                }
            };

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

            //For New Search Block
            $(document).on('click',function(e){
                $('.search-dropdown').hide();
            });

            $('.search-dropdown').on('click',function(e){
                e.stopPropagation(); 
            });

            $('.kksearch .input-group input').on('focus click',function(e){
                $('.search-dropdown').show();    
                e.stopPropagation(); 

            });

            $('.kksearch .input-group input').on('change keydown keyup paste move remove',function(){

                var $value = $(this).val();
                
                if($value.length == 0){
                    $('.default-suggest').show();
                    $('.autosuggest').hide();
                }else{
                    $('.autosuggest').show();
                    $('.default-suggest').hide();
                }
            });

            $('.open-search').on('click',function(e){
                
                e.stopPropagation(); 

                $('.kksearch').addClass('slideIn');
                $('.kksearch input').focus();

            });

            $('.close-search').on('click',function(){

                $('.kksearch').removeClass('slideIn');

            });

            //Clear input's value
            $('input').on('change keyup keydown paste move',function(){
                
                var $inputValue = $(this).val();
                var $clear = $(this).siblings('.clear-input');

                if($inputValue.length == 0){
                $clear.hide();
               } else{
                $clear.show();
               };

            });

           $('.clear-input').on('click',function(e){
                $(this).siblings('input').val('').focus();
                $(this).hide();
                $('.default-suggest').show();
                $('.autosuggest').hide();

                e.stopPropagation();
            });
        
            $('[data-toggle="tooltip"]').tooltip();

            $('.add-notice-user-btn').on('click',function(){
                $('#other-notice-email-area').slideDown();
                $('.other-notice-email-input input').focus();
            });

            //slideshow
            var img=$("#kk-slideshow");

            img.owlCarousel({

                items : 1,
                responsive:true,
                responsiveRefreshRate:200,
                responsiveBaseElement:window,
                autoHeight : false,
                lazyLoad:true,
                rewind:true
            });

            // Custom Navigation Events
            $(".next").click(function(){
                img.trigger('next.owl');
                view_item.trigger('next.owl');
            })
            $(".prev").click(function(){
                img.trigger('prev.owl');
                view_item.trigger('prev.owl');
            });

            //點擊編輯滑動至已選的套餐
            var $html = $('html,body');
            $('.view-option').on('click', function(){
                $html.animate({scrollTop:$('.option-item.selected').offset().top - 60}, 300);
            });

            //New option section DEMO用
            $('.select-option').on('click',function(){
                
                var $this = $(this),
                    $option = $this.parents('.option-item'),
                    $otherOption = $option.siblings(),
                    $bar = $('.booking-bar-content'),
                    $name = $option.find('.option-title').text(),
                    $price = $option.find('.product-pricing h4').text();
                
                //Select後該option加上藍色框
                $otherOption.removeClass('selected');
                $option.addClass('selected');

                //Select 後展開套餐需填寫欄位
                $otherOption.find('.option-booking').slideUp(200);
                $option.find('.option-booking').slideDown(300,function(){
                    //選擇套餐後滑動
                    $('.view-option').trigger('click');
                }); 

                //選擇套餐後side bar加上.active，打開 .view-option, .booking-price
                $bar.addClass('active');

                //選擇套餐後 side bar 換字
                $bar.find('.option-title').text($name);
                $bar.find('.booking-price .product-pricing h2').text($price);

            });
            
                //展開 Option details
            $('.detail-toggle').on('click',function(){
                $(this).toggleClass('active');
                $(this).siblings('.option-detail').slideToggle();
            });

                //模擬加減人數
            $('.counter').on('click', function(){
                
                var $counter = $(this),
                    $num = $(this).siblings('.counter-num').val();

                if($counter.hasClass('add')){
                    
                    var $newNum = parseInt($num) + 1;

                }else{
                    
                    if ($num > 0) {
                        $newNum = parseInt($num) - 1;
                    }else{
                        $newNum = 0;
                    }
                }
                
                $counter.siblings('.counter-num').val($newNum);

                //模擬book彈跳按鈕動畫
                $('#booking-bar .btn-primary.disabled').hide();
                $('.btn-book').show().addClass('enabled');

            });

            //下拉選單換字
            $('.input-dropdown-menu li a').on('click',function(){
                var $value = $(this).text();

                $(this).parent('li').addClass('active');
                $(this).parent('li').siblings('li').removeClass('active');
                $(this).parents('.input-dropdown-menu').siblings('.input-dropdown').find('span').text($value);

            });

            //關閉 error modal 後 reload 頁面
            $('#load_error_msg').on('hidden.bs.modal', function(){
                location.reload();
                $(window).scrollTop(0);
            });

            // Scroll spy tab
            $('body').scrollspy({
                target: '#product-scroll-spy',
                offset: 80,
            });

            // Scroll spy scroll 效果         
            $('#product-scroll-spy ul li a').on('click',function(e){
                
                var href = $(this).attr('href');
                var top = $(href).offset().top - 40;
                var time = 500;

                // 防止網址改變以及頁面彈跳
                e.preventDefault();
                
                $html.animate({scrollTop: top},time);

            });
            //滑動到評論區
            $('#review-num, #more-review, #review .pagination li a').click(function(){
                $('a[href=#review-spy]').trigger('click');
            });

            //選擇套餐滑動
            $('.choice-pkg').on('click',function(){
                $('a[href=#option-spy]').trigger('click');
            });

            
            //評論的翻譯效果
            $('.review-translated').hide();
            $('.btn-translate').on('click',function(){

                var $this = $(this);

                $this.html('<i class="fa fa-circle-o-notch fa-spin"></i> 翻譯中');

                setTimeout(function(){
                    $this.hide();
                    $this.siblings('.review-translated').fadeIn();
                }, 1000)
                
            });         

            //推薦商品的carousel
            var view_item=$("#kk-alsoview");

            view_item.owlCarousel({
              margin: 15,
              nav: true,
              dots: false,
              smartSpeed: 200,
              navText: ['<i class="fa fa-angle-left fa-4x"></i>','<i class="fa fa-angle-right fa-4x"></i>'],
              rewind:true,
              responsive: {
                0: {
                    autoWidth:true,
                    margin: 10,
                    nav: false,
                    },
                767: {
                    items: 2,
                    mouseDrag: false,
                    touchDrag: false,
                    },
                991: {
                    items: 3,
                    mouseDrag: false,
                    touchDrag: false,
                    },
                1200: {
                    items: 4,
                    mouseDrag: false,
                    touchDrag: false,
                    }
                }
            });

            //推薦產品小於四個時置中
            var count_group_list = $('#kk-alsoview .owl-item').length;
            if(count_group_list < 4){
                $('#kk-alsoview .owl-stage').css('margin','0 auto');
            }

            // 隱私權政策關閉
         $('.header-sheet .close').on('click', function(){
             $(this).parents('.header-sheet').remove();
         });

            $(window).on('scroll load', function(){

                var scrollTop = $(window).scrollTop();
                var $productWrapTop = $('.product-wrap').offset().top - 60;
                //var $recommendTop = $('.product-recommend').offset().top - 40;

                // scroll spy 在 .product-wrap 內顯示且固定於上方
                //if(scrollTop > $productWrapTop & scrollTop < $recommendTop){
                if(scrollTop > $productWrapTop){
                    $('#product-scroll-spy').addClass('active');
                }else{
                    $('#product-scroll-spy').removeClass('active');
                }

                // 小螢幕時立即訂購按鈕fixed效果
                var $optionOffset = $('.option-section').offset().top + $('.option-section').outerHeight(true);

            if(scrollTop > $optionOffset){
                    $('#choose-mobile').addClass('active');
                }else{
                    $('#choose-mobile').removeClass('active');
                }

                // 手機版不顯示
                if ($(window).width() < 768 ) return;

                // 電腦版
                if (scrollTop > 100) {
                $('.scroll-top-wrapper').addClass('show');
                } else {
                $('.scroll-top-wrapper').removeClass('show');
                }

            });

            $('.scroll-top-wrapper').on('click', scrollToTop);

        }); 

        function add_to_wishlist(){

            $('#add_wishlist').attr("disabled", true);
            $('#add_wishlist').html('<div class="dot-load"> <span class="dot dot-1 bg-white"></span> <span class="dot dot-2 bg-white"></span> <span class="dot dot-3 bg-white"></span></div> ');
            $('#in_wishlist').delay(500).show(0);
            $('#add_wishlist').delay(500).hide(0);
            $('#in_wishlist').attr("disabled", false);
            $('#in_wishlist').html('<i class="fa fa-heart"></i>已收藏');
        }

        function delete_wishlist(){

            $('#in_wishlist').attr("disabled", true);
            $('#in_wishlist').html('<div class="dot-load"> <span class="dot dot-1 bg-white"></span> <span class="dot dot-2 bg-white"></span> <span class="dot dot-3 bg-white"></span></div> ');
            $('#add_wishlist').delay(500).show(0);
            $('#in_wishlist').delay(500).hide(0);
            $('#add_wishlist').attr("disabled", false);
            $('#add_wishlist').html('<i class="fa fa-heart-o"></i>加入我的收藏');
        }

        function linking_to_check_email(){
            $('#checking_email').modal('show');
        }

        function add_to_noticelist(){

            $('#btn_add_to_noticelist').attr("disabled", true);
            $('#btn_add_to_noticelist').html('<div class="dot-load"> <span class="dot dot-1 bg-white"></span> <span class="dot dot-2 bg-white"></span> <span class="dot dot-3 bg-white"></span></div> ');
            $('#send_notice_btn').prop('disabled', true);
            $('#checking_email').modal('hide');
            $('#btn_add_to_noticelist').delay(500).hide(0);
            $('#btn_in_noticelist').delay(500).show(0);

        }

        function if_login(){

            $('#contact-agency--nonmember').modal('show');

        }

        function photo_slideshow(){
            $('#product-photo-modal').modal('show');
        }

        function scrollToTop() {
            verticalOffset = typeof(verticalOffset) != 'undefined' ? verticalOffset : 0;
            element = $('body');
            offset = element.offset();
            offsetTop = offset.top;
            $('html, body').animate({scrollTop: offsetTop}, 500, 'linear');
        } 

        function setMarkerSource(map, locations) {
            var image = {
                url: 'https://www.sit.kkday.com/assets/img/icon_locate_01.png',
                scaledSize: new google.maps.Size(24, 36),
            };

            for (var i = 0; i < locations.length; i++) {
                var place = locations[i];
                var myLatLng = new google.maps.LatLng(place[1], place[2]);
                var marker = new google.maps.Marker({
                    position: myLatLng,
                    map: map,
                    icon: image,
                    title: place[0],
                    zIndex: place[3],
                    draggable: true,
                });
            }
        }

        function scrollToInfoSection($element) {
          $('html, body').animate({ scrollTop: $($element).parent().offset().top - 40 }, 500);
        }
