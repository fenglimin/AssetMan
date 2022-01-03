from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from webdriver_manager.chrome import ChromeDriverManager
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import Select
import time

s = Service(ChromeDriverManager().install())
driver = webdriver.Chrome(service=s)
driver.get('https://www.citicbank.com/personal/investment/lcjdxcpxx/proxy/')
selectFundType = Select(driver.find_element_by_class_name('cms_proxy_cplb'))

# select by visible text
selectFundType.select_by_visible_text('代销理财产品')

selectFundName = Select(driver.find_element_by_class_name('cms_proxy_vague_srh_type'))

# select by visible text
selectFundName.select_by_visible_text('产品代码')

inputCtrl = driver.find_element_by_class_name('cms_proxy_vague_srh_input')
actionCtrl = driver.find_element_by_class_name('cms_proxy_srh_btn')
resultFile = open('中信代销净值.txt', 'w')

with open('中信代销产品.txt', 'r') as fundFile:
    fundList = fundFile.readlines()
    for fund in fundList:
        inputCtrl.clear()
        inputCtrl.send_keys(fund)
        actionCtrl.click()
        time.sleep(3)
        result = driver.find_elements_by_xpath("//td")
        resultFile.write(result[9].text)
        resultFile.write(' ')
        resultFile.write(fund)        

resultFile.close()
