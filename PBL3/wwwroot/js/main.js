/**
* Template Name: Bethany
* Updated: Mar 10 2023 with Bootstrap v5.2.3
* Template URL: https://bootstrapmade.com/bethany-free-onepage-bootstrap-theme/
* Author: BootstrapMade.com
* License: https://bootstrapmade.com/license/
*/
function myf() {
    alert('hello');
}
//(function() {
//  "use strict";

//  /**
//   * Easy selector helper function
//   */
//    const select = (el, all = false) => {
//        el = el.trim();
//        if (all) {
//            return $(el);
//        } else {
//            return $(el)[0];
//        }
//    }


//  /**
//   * Easy event listener function
//   */
//    const on = (type, el, listener, all = false) => {
//        let selectEl = select(el, all)
//        if (selectEl) {
//            if (all) {
//                selectEl.forEach(e => $(e).on(type, listener))
//            } else {
//                $(selectEl).on(type, listener)
//            }
//        }
//    }


//  /**
//   * Easy on scroll event listener
//   */
//    const onscroll = (el, listener) => {
//        $(el).scroll(listener);
//    }


//  /**
//   * Navbar links active state on scroll
//   */
//    let navbarlinks = $('#navbar .scrollto')
//    const navbarlinksActive = () => {
//        let position = $(window).scrollTop() + 200
//        navbarlinks.each(function () {
//            let navbarlink = $(this);
//            if (!navbarlink.attr('href')) return
//            let section = $(navbarlink.attr('href'))
//            if (!section.length) return
//            if (position >= section.offset().top && position <= (section.offset().top + section.outerHeight())) {
//                navbarlink.addClass('active')
//            } else {
//                navbarlink.removeClass('active')
//            }
//        })
//    }

//    $(document).ready(function () {
//        navbarlinksActive();
//    });

//    $(window).scroll(function () {
//        navbarlinksActive();
//    });

//  /**
//   * Scrolls to an element with header offset
//   */
//    const scrollto = (el) => {
//        let header = $('#header');
//        let offset = header.outerHeight();

//        let elementPos = $(el).offset().top;
//        $('html, body').animate({
//            scrollTop: elementPos - offset
//        }, 1000);
//    }


//  /**
//   * Toggle .header-scrolled class to #header when page is scrolled
//   */
//    let $selectHeader = $('#header');
//    if ($selectHeader.length) {
//        const headerScrolled = () => {
//            if ($(window).scrollTop() > 100) {
//                $selectHeader.addClass('header-scrolled');
//            } else {
//                $selectHeader.removeClass('header-scrolled');
//            }
//        };
//        $(window).on('load', headerScrolled);
//        $(document).on('scroll', headerScrolled);
//    }


//  /**
//   * Back to top button
//   */
//    let $backtotop = $('.back-to-top');
//    if ($backtotop.length) {
//        const toggleBacktotop = () => {
//            if ($(window).scrollTop() > 100) {
//                $backtotop.addClass('active');
//            } else {
//                $backtotop.removeClass('active');
//            }
//        };
//        $(window).on('load', toggleBacktotop);
//        $(document).on('scroll', toggleBacktotop);
//    }


//  /**
//   * Mobile nav toggle
//   */
//    $(document).on('click', '.mobile-nav-toggle', function (e) {
//        $('#navbar').toggleClass('navbar-mobile');
//        $(this).toggleClass('bi-list bi-x');
//    });

//  /**
//   * Mobile nav dropdowns activate
//   */
//    $(document).on('click', '.navbar .dropdown > a', function (e) {
//        if ($('#navbar').hasClass('navbar-mobile')) {
//            e.preventDefault();
//            $(this).next().toggleClass('dropdown-active');
//        }
//    });

//  /**
//   * Scrool with ofset on links with a class name .scrollto
//   */
//    $(document).on('click', '.scrollto', function (e) {
//        if ($(this.hash).length) {
//            e.preventDefault();

//            let $navbar = $('#navbar');
//            if ($navbar.hasClass('navbar-mobile')) {
//                $navbar.removeClass('navbar-mobile');
//                let $navbarToggle = $('.mobile-nav-toggle');
//                $navbarToggle.toggleClass('bi-list bi-x');
//            }
//            scrollto(this.hash);
//        }
//    });


//  /**
//   * Scroll with ofset on page load with hash links in the url
//   */
//    $(window).on('load', () => {
//        if (window.location.hash) {
//            if ($(window.location.hash).length) {
//                $('html, body').animate({ scrollTop: $(window.location.hash).offset().top }, 'slow');
//            }
//        }
//    });

//  /**
//   * Testimonials slider
//   */
//    $('.testimonials-slider').each(function () {
//        $(this).swiper({
//            speed: 600,
//            loop: true,
//            autoplay: {
//                delay: 5000,
//                disableOnInteraction: false
//            },
//            slidesPerView: 'auto',
//            pagination: {
//                el: '.swiper-pagination',
//                type: 'bullets',
//                clickable: true
//            },
//            breakpoints: {
//                320: {
//                    slidesPerView: 1,
//                    spaceBetween: 20
//                },

//                1200: {
//                    slidesPerView: 2,
//                    spaceBetween: 20
//                }
//            }
//        });
//    });


//  /**
//   * Animation on scroll
//   */
//    $(window).on('load', function () {
//        AOS.init({
//            duration: 1000,
//            easing: 'ease-in-out',
//            once: true,
//            mirror: false
//        });
//    });
//})
//$('.testimonials-slider').each(function () {
//    $(this).swiper({
//        speed: 600,
//        loop: true,
//        autoplay: {
//            delay: 5000,
//            disableOnInteraction: false
//        },
//        slidesPerView: 'auto',
//        pagination: {
//            el: '.swiper-pagination',
//            type: 'bullets',
//            clickable: true
//        },
//        breakpoints: {
//            320: {
//                slidesPerView: 1,
//                spaceBetween: 20
//            },

//            1200: {
//                slidesPerView: 2,
//                spaceBetween: 20
//            }
//        }
//    });
//});


//new Swiper('.testimonials-slider', {
//    speed: 600,
//    loop: true,
//    autoplay: {
//        delay: 5000,
//        disableOnInteraction: false
//    },
//    slidesPerView: 'auto',
//    pagination: {
//        el: '.swiper-pagination',
//        type: 'bullets',
//        clickable: true
//    },
//    breakpoints: {
//        320: {
//            slidesPerView: 1,
//            spaceBetween: 20
//        },
//        1200: {
//            slidesPerView: 2,
//            spaceBetween: 20
//        }
//    }
//});
var mySwiper = new Swiper('.testimonials-slider', {
    // Các Parameters
    direction: 'vertical',
    loop: true,

    // N?u c?n pagination
    pagination: {
        el: '.swiper-pagination',
    },

    // N?u c?n navigation
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },

    // N?u c?n scrollbar
    scrollbar: {
        el: '.swiper-scrollbar',
    },
})
