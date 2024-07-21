$('.owl-carousel').owlCarousel({
    loop:true,
    nav:false,
    dots:false,
    items: 1,
    autoplay: true,
    
})

const observer = new  IntersectionObserver((entries)=>{
 entries.forEach((entry)=>{
   console.log(entry)
   if(entry.isIntersecting){
     entry.target.classList.add('show')
   }
   else{
    entry.target.classList.remove('show')
   }
 });
});
const hiddenElements =document.querySelectorAll('.hidden');
hiddenElements.forEach((el)=>observer.observe(el));




$(".color_wrap ul li").each(function(item){
  var color = $(this).attr("data-color");
  $(this).css("backgroundColor", color);
});

$(".color_wrap ul li").each(function(item){
  $(this).click(function(){
    $(this).parents(".product_item").find(".color_wrap ul li").removeClass("active");
    $(this).addClass("active");
    var img_src = $(this).attr("data-src");
    $(this).parents(".product_item").find("img").attr("src", img_src);
  });
})





let cartIcon = document.querySelector('#cart-icon')
let cart = document.querySelector('.cart')
let closeCart = document.querySelector('#close-cart')

cartIcon.onclick = () => {
  cart.classList.add("active");
};

closeCart.onclick = () => {
  cart.classList.remove("active");
};


let whislistIcon= document.querySelector('#whislist-icon')
let whislist = document.querySelector('.whislist')
let closeWhislist = document.querySelector('#close-whislist')

whislistIcon.onclick = () => {
  whislist.classList.add("active");
};

closeWhislist.onclick = () => {
  whislist.classList.remove("active");
};



const menuHamburger = document.querySelector(".menu-hamburger");
const navLinks =document.querySelector("#navlinks");

menuHamburger.addEventListener('click',()=>{
 navLinks.classList.toggle('mobile-menu')
});




$(window).scroll(function () {
  let scrollPx = $(window).scrollTop()
  if (scrollPx > 500) {
      $("#backToTop").fadeIn(450)
      $("#backToTop").css("display", "flex")
  } else {
      $("#backToTop").fadeOut(450)

  }

})

$("#backToTop").click(function () {
  $(window).scrollTop(0)
})


let searchBtn = document.querySelector('.searchBtn');

let closeBtn = document.querySelector('.closeBtn');

let searchBox = document.querySelector('.searchBox');

searchBtn.onclick = function(){
  searchBox.classList.add('active')
}


closeBtn.onclick = function(){
  searchBox.classList.remove('active')
}








window.addEventListener('load',fg_load)

function fg_load(){
  document.getElementById('loading').style.display='none'
}



(function( $ ) {

	$.fn.numberstyle = function(options) {

		/*
		 * Default settings
		 */
		var settings = $.extend({
			value: 0,
			step: undefined,
			min: undefined,
			max: undefined
		}, options );

		/*
		 * Init every element
		 */
		return this.each(function(i) {
				
      /*
       * Base options
       */
      var input = $(this);
          
			/*
       * Add new DOM
       */
      var container = document.createElement('div'),
          btnAdd = document.createElement('div'),
          btnRem = document.createElement('div'),
          min = (settings.min) ? settings.min : input.attr('min'),
          max = (settings.max) ? settings.max : input.attr('max'),
          value = (settings.value) ? settings.value : parseFloat(input.val());
      container.className = 'numberstyle-qty';
      btnAdd.className = (max && value >= max ) ? 'qty-btn qty-add disabled' : 'qty-btn qty-add';
      btnAdd.innerHTML = '+';
      btnRem.className = (min && value <= min) ? 'qty-btn qty-rem disabled' : 'qty-btn qty-rem';
      btnRem.innerHTML = '-';
      input.wrap(container);
      input.closest('.numberstyle-qty').prepend(btnRem).append(btnAdd);

      /*
       * Attach events
       */
      // use .off() to prevent triggering twice
      $(document).off('click','.qty-btn').on('click','.qty-btn',function(e){
        
        var input = $(this).siblings('input'),
            sibBtn = $(this).siblings('.qty-btn'),
            step = (settings.step) ? parseFloat(settings.step) : parseFloat(input.attr('step')),
            min = (settings.min) ? settings.min : ( input.attr('min') ) ? input.attr('min') : undefined,
            max = (settings.max) ? settings.max : ( input.attr('max') ) ? input.attr('max') : undefined,
            oldValue = parseFloat(input.val()),
            newVal;
        
        //Add value
        if ( $(this).hasClass('qty-add') ) {   
          
          newVal = (oldValue >= max) ? oldValue : oldValue + step,
          newVal = (newVal > max) ? max : newVal;
          
          if (newVal == max) {
            $(this).addClass('disabled');
          }
          sibBtn.removeClass('disabled');
         
        //Remove value
        } else {
          
          newVal = (oldValue <= min) ? oldValue : oldValue - step,
          newVal = (newVal < min) ? min : newVal; 
          
          if (newVal == min) {
            $(this).addClass('disabled');
          }
          sibBtn.removeClass('disabled');
          
        }
          
        //Update value
        input.val(newVal).trigger('change');
            
      });
      
      input.on('change',function(){
        
        const val = parseFloat(input.val()),
              min = (settings.min) ? settings.min : ( input.attr('min') ) ? input.attr('min') : undefined,
            	max = (settings.max) ? settings.max : ( input.attr('max') ) ? input.attr('max') : undefined;
        
        if ( val > max ) {
          input.val(max);   
        }
        
        if ( val < min ) {
          input.val(min);
        }
      });
      
		});
	};

  
  /*
   * Init
   */
  
	$('.numberstyle').numberstyle();

}( jQuery ));




let subMenu = document.getElementById('subMenu');

function toogleUser(){
  subMenu.classList.toggle("open-menu");
}



gsap.registerPlugin(ScrollTrigger);

gsap.to("progress", {
    value: 100,
    scrollTrigger: {
        scrub: 0.5
    }
});
