class Semaphore:
    colors = ["red", "yellow", "green"]

    def __init__(self, color:str = "red"):
        if color in self.colors:
            self.color = color
        else:
            raise ValueError("Neplatná barva semaforu")

    def __str__(self):
        return self.color

    def next_color(self):
        """
        vrací semafor s následující barvou v sekvenci přepínání světel
        """
        self._index = (self.colors.index(self.color) + 1) % 3 
        return self.colors[self._index]
    

    def __iter__(self):
        self._index = self.colors.index(self.color)
        return self
    

    def __next__(self):
        self.color = self.next_color()
    
    
    @property
    def stop(self):
        return "Stop!" if self.color == "red" else "Go"
    
    
    def __eq__(self, other):
        if isinstance(other, Semaphore):
            return "Equal" if self.color == other.color else "Unequal"
        else:
            raise ValueError("Compared object is not a Semaphore")



semafor = Semaphore()
i = 0
for color in semafor:
    print(semafor)
    i += 1
    if i > 10:
        break