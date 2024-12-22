document.addEventListener("DOMContentLoaded", function () {

    const siparisEkleButtons = document.querySelectorAll(".siparis-ekle");
    siparisEkleButtons.forEach(button => {
        button.addEventListener("click", function () {
            const masaId = this.getAttribute("data-id");

            // Fetch API ile controller'a istek gönder
            fetch(`/Masa/_SiparisEkle/${masaId}`)
                .then(response => response.text())
                .then(data => {
                    // Partial view'i modala yerleştir
                    document.getElementById("siparisModalContainer").innerHTML = data;
                    // Modalı göster
                    const siparisModal = new bootstrap.Modal(document.getElementById("siparisModal"));
                    siparisModal.show();
                })
                .catch(error => {
                    alert("Bir hata oluştu!");
                });
        });
    });
});


function addEventListenersToModal() {
    document.querySelectorAll('input[name^="SelectedSiparisDetaylar"]').forEach(checkbox => {
        checkbox.addEventListener('change', updateTotalAmount);
    });

    document.getElementById('selectAll')?.addEventListener('change', function () {
        const checkboxes = document.querySelectorAll('input[name^="SelectedSiparisDetaylar"]');
        checkboxes.forEach(checkbox => checkbox.checked = this.checked);
        updateTotalAmount();
    });
}

function updateTotalAmount() {
    const checkboxes = document.querySelectorAll('input[name^="SelectedSiparisDetaylar"]:checked');
    let total = 0;
    checkboxes.forEach(checkbox => {
        const row = checkbox.closest('tr');
        const totalCell = row.querySelector('td:last-child').innerText.replace('TL', '').trim().replace(',', '.');
        total += parseFloat(totalCell);
    });
    document.getElementById('totalAmount').innerText = `Toplam: ${total.toFixed(2).replace('.', ',')} TL`;
}



function renderCreateMusteriModal() {
    fetch(`/Masa/_MusteriEkle/`)
        .then(response => response.text())
        .then(data => {
            document.getElementById("musteriEkleModalContainer").innerHTML = data;
            const musteriModal = new bootstrap.Modal(document.getElementById("createMusteriModal"));
            musteriModal.show();
        })
        .catch(error => {
            alert("Bir hata oluştu!");
        });
}
function addSiparisDetay() {
    const firstDetail = document.querySelector('.siparis-detay');
    const clonedDetail = firstDetail.cloneNode(true);

    const inputs = clonedDetail.querySelectorAll('input, select');
    inputs.forEach(input => {
        if (input.tagName === 'SELECT') {
            input.selectedIndex = 0;
        } else if (input.tagName === 'INPUT') {
            input.value = '';
        }
    });

    const detaylar = document.querySelectorAll('.siparis-detay').length;
    clonedDetail.querySelectorAll('select, input').forEach(input => {
        const name = input.getAttribute('name');
        if (name) {
            input.setAttribute('name', name.replace(/\[\d+\]/, `[${detaylar}]`));
        }
    });

    document.getElementById('siparisDetayContainer').appendChild(clonedDetail);
}

function removeSiparisDetay(button) {
    const detaylar = document.querySelectorAll('.siparis-detay');
    if (detaylar.length > 1) {
        button.closest('.siparis-detay').remove();
    } else {
        alert('En az bir sipariş detay kalmalıdır.');
    }
}