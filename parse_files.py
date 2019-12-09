#parse an image folder to generate Image object declarations
import os

def GetName(file):
    return os.path.splitext(file)[0].capitalize().replace("_","")

folder = input("Please type the name of the folder: ")
files = os.listdir(folder)

output = open("seed_code.txt","w")

i = 7
for file in files:
    output.write('NewImages.Add(new Image {{ ID = {3}, Source = "/images/{2}/{0}", ImageName = "{1}", Category = "None", ReferenceCount = 0 }});\n'.format(file, GetName(file),folder,i))
    i += 1

output.close()
