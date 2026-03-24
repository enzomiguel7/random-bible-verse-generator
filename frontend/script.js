const API_URL = 'http://localhost:5186';

const ALL_TAGS = [
    'grace', 'weakness', 'power_of_God', 'christ', 'mary', 
    'old_testament', 'new_testament', 'salvation', 'sin', 'prayer', 'wisdom'
];

let selectedTags = new Set();

const textEl = document.getElementById('verse-text');
const refEl = document.getElementById('verse-reference');
const tagsContainer = document.getElementById('available-tags');
const tagsDisplayEl = document.getElementById('verse-tags-display');
const loadingEl = document.getElementById('loading-indicator');
const btn = document.getElementById('btn-reveal');

function renderTagButtons() {
    tagsContainer.innerHTML = '';
    ALL_TAGS.forEach(tag => {
        const btnTag = document.createElement('button');
        btnTag.className = 'px-4 py-2 rounded-xl bg-white/5 border border-white/10 text-sm transition-all hover:bg-white/10 active:scale-90';
        btnTag.innerText = tag.replace(/_/g, ' ');
        
        btnTag.onclick = () => {
            if (selectedTags.has(tag)) {
                selectedTags.delete(tag);
                btnTag.classList.remove('tag-active');
            } else {
                selectedTags.add(tag);
                btnTag.classList.add('tag-active');
            }
        };
        tagsContainer.appendChild(btnTag);
    });
}

async function loadVerse() {
    const startTime = Date.now();
    const minWait = 700; 

    try {
        btn.disabled = true;
        loadingEl.classList.remove('opacity-0', 'pointer-events-none');
        textEl.style.filter = 'blur(8px)';
        textEl.style.opacity = '0.3';

        let url = `${API_URL}/randomverse`;
        if (selectedTags.size > 0) {
            const params = new URLSearchParams();
            selectedTags.forEach(t => params.append('tag', t));
            url += `?${params.toString()}`;
        }

        const response = await fetch(url);
        
        const elapsed = Date.now() - startTime;
        if (elapsed < minWait) await new Promise(r => setTimeout(r, minWait - elapsed));

        if (response.status === 404) {
            textEl.innerText = "No verse found with these tags. Try another combination!";
            refEl.innerText = "";      
            tagsDisplayEl.innerHTML = ""; 
            return; 
        }

        if (!response.ok) throw new Error("API response error");

        const data = await response.json();
        
        textEl.innerText = data.text;
        refEl.innerText = data.reference || `${data.book} ${data.chapter}:${data.verse}`;
        
        tagsDisplayEl.innerHTML = '';
        if (data.tags) {
            data.tags.forEach(t => {
                const s = document.createElement('span');
                s.className = 'px-3 py-1 rounded-lg bg-white/5 border border-white/10 text-[10px] uppercase tracking-widest text-primary/80 font-bold whitespace-nowrap';
                s.innerText = t.replace(/_/g, ' ');
                tagsDisplayEl.appendChild(s);
            });
        }

    } catch (e) {
        console.error("Detailed error:", e);
        textEl.innerText = "Error connecting to the API. Check if the C# server is running at " + API_URL;
        refEl.innerText = "";
        tagsDisplayEl.innerHTML = "";
    } finally {
        loadingEl.classList.add('opacity-0', 'pointer-events-none');
        textEl.style.filter = 'none';
        textEl.style.opacity = '1';
        btn.disabled = false;
    }
}

renderTagButtons();
btn.addEventListener('click', loadVerse);
window.addEventListener('DOMContentLoaded', loadVerse);