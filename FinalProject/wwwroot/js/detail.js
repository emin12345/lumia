$(document).ready(function() {
    $('.minus').click(function() {
      var $input = $(this).siblings('input');
      var count = parseInt($input.val()) - 1;
      count = count < 1 ? 1 : count;
      $input.val(count).change();
      return false;
    });
  
    $('.plus').click(function() {
      var $input = $(this).siblings('input');
      var count = parseInt($input.val()) + 1;
      $input.val(count).change();
      return false;
    });
  });
  