document.addEventListener('DOMContentLoaded', function () {
    const liveSearch = document.getElementById('liveSearch');
    if (liveSearch) {
        liveSearch.addEventListener('input', function () {
            const term = this.value.trim().toLowerCase();
            let visible = 0;
            document.querySelectorAll('[data-game-card]').forEach(function (card) {
                const title = (card.getAttribute('data-title') || '').toLowerCase();
                const match = title.includes(term);
                card.style.display = match ? '' : 'none';
                if (match) visible++;
            });
            const empty = document.getElementById('emptyState');
            if (empty) empty.style.display = visible === 0 ? '' : 'none';
        });
    }

    document.querySelectorAll('[data-autosubmit]').forEach(function (el) {
        el.addEventListener('change', function () {
            const form = el.closest('form');
            if (form) form.submit();
        });
    });

    const coverInput = document.getElementById('coverFile');
    const coverPreview = document.getElementById('coverPreview');
    if (coverInput && coverPreview) {
        coverInput.addEventListener('change', function () {
            const file = this.files && this.files[0];
            if (!file) return;
            const reader = new FileReader();
            reader.onload = e => {
                coverPreview.src = e.target.result;
                coverPreview.style.display = 'block';
            };
            reader.readAsDataURL(file);
        });
    }

    const alertBox = document.querySelector('.gs-alert');
    if (alertBox) {
        setTimeout(() => {
            alertBox.classList.remove('show');
        }, 5000);
    }
});
