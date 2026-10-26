const URL = '';
let entries = [];

const dateAndTimeToDate = (dateString, timeString) => {
    return new Date(`${dateString}T${timeString}`).toISOString();
};

const createEntry = (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const entry = {};
    entry['checkIn'] = dateAndTimeToDate(formData.get('checkInDate'), formData.get('checkInTime'));
    entry['checkOut'] = dateAndTimeToDate(formData.get('checkOutDate'), formData.get('checkOutTime'));

    fetch(`${URL}/entries`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(entry)
    }).then((result) => {
        result.json().then((entry) => {
            entries.push(entry);
            renderEntries();
        });
    });
};

const deleteEntry = (id) => {
    fetch(`${URL}/entries/${id}`, {
        method: 'DELETE'}).then((result) => {
        if (result.ok) {
            entries = entries.filter((e) => e.id !== id);
            renderEntries();
        }
    });
};

const updateEntry = (id, entry) => {
    fetch(`${URL}/entries/${id}`, {
        method: 'PUT',
        headers: {
            contentType: 'application/json'
        },
        body: JSON.stringify(entry)
        }).then((result) => {
        if (result.ok) {
            entries = entries.map((e) => e.id === id ? { ...e, ...entry } : e);
            renderEntries();
        }});
};

fetch(`${URL}/entries/{id}`, {
    method: 'PUT',
    headers: {
        'Content-Type': 'application/json'
    },
    body: JSON.stringify(entry)
}).then((result) => {
    if (result.ok) {
        entries = entries.map((e) => e.id === id ? { ...e, ...entry } : e);
        renderEntries();
    }
});

const indexEntries = () => {
    fetch(`${URL}/entries`, {
        method: 'GET'
    }).then((result) => {
        result.json().then((result) => {
            entries = result;
            renderEntries();
        });
    });
    renderEntries();
};

const createCell = (text) => {
    const cell = document.createElement('td');
    cell.innerText = text;
    return cell;
};

const renderEntries = () => {
    const display = document.querySelector('#entryDisplay');
    display.innerHTML = '';
    entries.forEach((entry) => {
        const row = document.createElement('tr');
        row.appendChild(createCell(entry.id));
        row.appendChild(createCell(new Date(entry.checkIn).toLocaleString()));
        row.appendChild(createCell(new Date(entry.checkOut).toLocaleString()));
        display.appendChild(row);

        const deleteButton = document.createElement('button');
        const updateButton = document.createElement('button');
        deleteButton.innerText = 'Delete';
        updateButton.innerText = 'Update';
        deleteButton.addEventListener('click', () => deleteEntry(entry.id));
        updateButton.addEventListener('click', () => {
            const newCheckIn = prompt('Enter new check-in date and time:', new Date(entry.checkIn).toLocaleString());
            const newCheckOut = prompt('Enter new check-out date and time:', new Date(entry.checkOut).toLocaleString());

            if (newCheckIn && newCheckOut) {
                const updatedEntry = {
                    checkIn: dateAndTimeToDate(newCheckIn.split(' ')[0], newCheckIn.split(' ')[1]),
                    checkOut: dateAndTimeToDate(newCheckOut.split(' ')[0], newCheckOut.split(' ')[1])
                };
                updateEntry(entry.id, updatedEntry);
            }
        });
        row.appendChild(deleteButton);
        row.appendChild(updateButton);
    });
};

document.addEventListener('DOMContentLoaded', function(){
    const createEntryForm = document.querySelector('#createEntryForm');
    createEntryForm.addEventListener('submit', createEntry);
    indexEntries();
});