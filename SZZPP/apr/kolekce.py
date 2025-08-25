# kód má ke každé položce modifikovatelné sekvence s celočíselnými prvky
# přičíst 1 je-li položka sudá

data = list(range(1, 10))

if not all(isinstance(item, (int, float)) for item in data):
    raise ValueError("Všechny prvky musí být čísla")

new_data = data[:]

for i in range(1, len(data), 2):
    new_data[i] += 1

print("Původní data: ", data)
print("Upravená data: ", new_data)


