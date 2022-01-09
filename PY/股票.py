from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from webdriver_manager.chrome import ChromeDriverManager
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import Select
import time

s = Service(ChromeDriverManager().install())
driver = webdriver.Chrome(service=s)
resultFile = open('股票净值.txt', 'w')

with open('股票产品.txt', 'r') as fundFile:
    fundList = fundFile.readlines()
    for fund in fundList:
        driver.get('http://quote.eastmoney.com/' + fund + '.html')
        time.sleep(3)
        netWorth = driver.find_element_by_id('price9')
        resultFile.write(netWorth.get_attribute('innerHTML'))
        resultFile.write(' ')
        resultFile.write(fund)
resultFile.close()
driver.close();
