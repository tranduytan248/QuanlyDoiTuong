(function ($) {

  function getOptions(options) {
    options = options || {};
    options.confirmationSelector = options.confirmationSelector || 'span';
    options.errorSelector = options.errorSelector || null;
    options.errorMessage  = options.errorMessage  || 'The entered number is invalid';
    return options;
  }

  function numberInputFormatter(options) {
    options = getOptions(options);
    $(this).on('keyup change blur focus', function (e) {
      formatInput($(e.target), options)
    });
    $(this).each(function () {
      formatInput($(this), options);
    });
    return this;
  }

  function formatInput($field, options) {
    var value = $field.val();
    var formattedNumber = formatNumber(value);
    var $confirmationEl = getSiblingElementFromField($field, options.confirmationSelector);
    var $errorEl = getSiblingElementFromField($field, options.errorSelector);
    if (formattedNumber === null) {
      $errorEl.text(options.errorMessage);
      $confirmationEl.text('');
    } else {
      $errorEl.text('');
      $confirmationEl.text(formattedNumber || '');
    }
  }

  /**
   * Get the sibling element from a base element by selector.
   *
   * @param jQuery $field The input
   * @param String selector A CSS selector to find the sibling element
   * @return jQuery
   */
  function getSiblingElementFromField($field, selector) {
    return $field.parent().find(selector);
  }

  /**
   * Format a number without providing the number of decimal places.
   *
   * This is called "simple" because it will automatically count the number of
   * decimal places in the provided number.
   *
   * @param Number|String number The number to format
   * @param String [decimal] The decimal separator, a period by default
   * @param String [separator] The places separator, a comma by default
   * @return String A properly formatted number
   */
  function formatNumber(number, decimal, separator) {
    var decimals = countDecimals(number, decimal);
    return formatNumberBase(number, decimals, decimal, separator);
  }

  /**
   * Count the number of decimal places in a number.
   *
   * e.g.:
   * 1234 returns 0
   * 1234.56 returns 2
   *
   * @param Number|String number The number with decimal places to count
   * @param String [decimal] The decimal separator, a period by default
   * @return Number
   */
  function countDecimals(number, decimal) {
    decimal = decimal || '.';
    number = ('' + number).split(decimal);
    return number[1] ? number[1].length : 0;
  }

  /**
   * Number formatter.
   *
   * Inspired by http://stackoverflow.com/questions/149055/how-can-i-format-numbers-as-money-in-javascript
   *
   * n = amount
   * c = # digits to the right of the decimal
   * d = decimal (defaults to period)
   * t = separator (defaults to comma)
   * @return String  formatted number
   */
  function formatNumberBase(n, c, d, t) {
    n = parseFloat(n);
    if (isNaN(n)) {
      return null;
    }
    var c = isNaN(c = Math.abs(c)) ? 2 : c,
      d = d == undefined ? "." : d,
      t = t == undefined ? "," : t,
      s = n < 0 ? "-" : "",
      i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
      j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
  }

  $.fn.numberInputFormatter = numberInputFormatter;

}(jQuery));
