let filterIcon = document.querySelector('#filter-icon')
let filter = document.querySelector('.filter')
let closeFilter = document.querySelector('#close-filter')

filterIcon.onclick = () => {
 filter.classList.add("active");
};

closeFilter.onclick = () => {
 filter.classList.remove("active");
};







const rangeInput = document.querySelectorAll(".range-input input"),
  priceInput = document.querySelectorAll(".price-input input"),
  range = document.querySelector(".slider .progress");
let priceGap = 30;

priceInput.forEach((input) => {
  input.addEventListener("input", (e) => {
    let minPrice = parseInt(priceInput[0].value),
      maxPrice = parseInt(priceInput[1].value);

    if (maxPrice - minPrice >= priceGap && maxPrice <= rangeInput[1].max) {
      if (e.target.className === "input-min") {
        rangeInput[0].value = minPrice;
        range.style.left = (minPrice / rangeInput[0].max) * 100 + "%";
      } else {
        rangeInput[1].value = maxPrice;
        range.style.right = 100 - (maxPrice / rangeInput[1].max) * 100 + "%";
      }
    }
  });
});

rangeInput.forEach((input) => {
  input.addEventListener("input", (e) => {
    let minVal = parseInt(rangeInput[0].value),
      maxVal = parseInt(rangeInput[1].value);

    if (maxVal - minVal < priceGap) {
      if (e.target.className === "range-min") {
        rangeInput[0].value = maxVal - priceGap;
      } else {
        rangeInput[1].value = minVal + priceGap;
      }
    } else {
      priceInput[0].value = minVal;
      priceInput[1].value = maxVal;
      range.style.left = (minVal / rangeInput[0].max) * 100 + "%";
      range.style.right = 100 - (maxVal / rangeInput[1].max) * 100 + "%";
    }
  });
});


document.addEventListener('DOMContentLoaded', function () {
    var headeraccorList = document.querySelectorAll('.headeraccor');

    for (var i = 0; i < headeraccorList.length; i++) {
        headeraccorList[i].addEventListener('click', function () {
            var siblings = Array.from(this.parentNode.children);
            var bodyaccorList = Array.from(this.parentNode.querySelectorAll('.bodyaccor'));
            var icons = Array.from(this.parentNode.querySelectorAll('.headeraccor i'));
            var isActive = this.classList.contains('active');

            for (var j = 0; j < siblings.length; j++) {
                if (siblings[j] !== this) {
                    var icon = siblings[j].querySelector('i');
                    if (icon) {
                        icon.classList.remove('fa-chevron-up');
                        icon.classList.add('fa-chevron-down');
                    }
                }
            }

            for (var k = 0; k < bodyaccorList.length; k++) {
                if (bodyaccorList[k] !== this.nextElementSibling) {
                    bodyaccorList[k].style.display = 'none';
                }
            }

            var currentIcon = this.querySelector('i');
            if (currentIcon) {
                currentIcon.classList.toggle('fa-chevron-down');
                currentIcon.classList.toggle('fa-chevron-up');
            }

            var nextElement = this.nextElementSibling;
            if (nextElement) {
                nextElement.style.display = isActive ? 'none' : 'block';
            }

            this.classList.toggle('active');
        });
    }
});



let field = document.querySelector('#sss');
let li = Array.from(field.children);
let select = document.getElementById('Featureda');
let ar = [];

for (let i of li) {
    const last = i.lastElementChild;
    const x = last.textContent.trim();
    const y = parseFloat(x.replace('$', ''));

    if (!isNaN(y)) {
        i.setAttribute('data-price', y);
        ar.push(i);
    }
}

select.onchange = sortingValue;

function sortingValue() {
    if (this.value === 'Default') {
        while (field.firstChild) { field.removeChild(field.firstChild); }
        field.append(...ar);
    }
    if (this.value === 'Pricelow') {
        sortElem(field, li, true);
    }
    if (this.value === 'PriceHigh') {
        sortElem(field, li, false);
    }
}

function sortElem(field, li, asc) {
    let dm, sortli;
    dm = asc ? 1 : -1;
    sortli = li.sort((a, b) => {
        const ax = a.getAttribute('data-price');
        const bx = b.getAttribute('data-price');
        return ax > bx ? (1 * dm) : (-1 * dm);
    });
    while (field.firstChild) { field.removeChild(field.firstChild); }
    field.append(...sortli);
}
